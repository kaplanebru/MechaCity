using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Enums;
using Network;
using Unity.Netcode;
using UnityEngine;
using Teams;
using GameUI;
using JetBrains.Annotations;
using Testing;
using Towers;


namespace Turn
{
    public class TurnManager : MonoBehaviour ////NetworkBehaviour
    {
        public static int TurnTracker => _turnTracker; //no setter
        private static int _turnTracker = 0;

        private BaseTurnState currentState;
        private BaseTurnState previousState;
        
        private TurnStateHolder _stateHolder = new();
        private BlueprintEventHandler bpEventHandler;
        
        private CombatHelper _combatHelper;
        private TurnHelper turnHelper = new();

        private bool firstTurn = true;



        private void OnEnable()
        {
            Eventbus.TeamEvents.OnTeamsSet += SetTurnTeams;

            NetworkEventbus.OnAllClientsSet += FirstTurn;
            NetworkEventbus.RequestEvents.OnStateChangeRequestByServer += ChangeStateBySystem;
            Eventbus.CombatEvents.OnCombatTerminated += EndTurn;
            
            UIEventbus.OnButtonCall += ShowButtonRequest; //todo: sadece state'i tutan bir kod olabilir, state'e göre action alan
            UIEventbus.OnButtonClicked += StateChangeRequestByUser;
            Eventbus.StateEvents.OnStateChangeWithoutInteraction += StateChangeRequestByUser;
            
            bpEventHandler = new BlueprintEventHandler(this);
        }

   
        private void ShowButtonRequest(bool enable)
        {
            UIEventbus.OnShowButtonRequest?.Invoke(enable, currentState.StateType);
        }

        private void Initialize()
        {
            _stateHolder.RegisterStates();

            UIEventbus.TurnEvents.OnInitialize?.Invoke();
        }

        void SetTurnTeams(Team[] teams)
        {
            turnHelper.TurnTeams = new Dictionary<TeamState, Team>()
            {
                {TeamState.CurrentTeam, teams[0]},
                {TeamState.RivalTeam, teams[1]},
            };
        }

        void FirstTurn(params object[] args)
        {
            Initialize();
            
            _combatHelper = ((ExitState) _stateHolder.GetStateByType(TurnStateType.Exit)).combatHelper;
            _combatHelper.Subscribe(null);
            _combatHelper.SetCombatPairs();

            NewTurn();
            firstTurn = false;
        }

        void NewTurn()
        {
            _turnTracker++;
            print("turn track: " + _turnTracker);
            turnHelper.ManageInput();
            SetFirstState();
        }

        void SetFirstState()
        {
            if (firstTurn)
                SetNewState(_stateHolder.GetStateByType(TurnStateType.Selection));
            else
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(TurnStateType.Selection);
            
        }
        
        private void StateChangeRequestByUser()
        {
            currentState.ProcessExecutionWithSelection(); //execute bp
            
            if(currentState.StateType == TurnStateType.Intruder)
                GetPreviousState();
            else
                GetNextState();
        }

        public void GetNextState()
        {
            var nextType = _stateHolder.States[turnHelper.GetNextStateId(currentState.StateId)].StateType;
            NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(nextType);

        }
        public void GetPreviousState()
        {
            var previousType = previousState?.StateType ?? TurnStateType.Exit; //todo: check
            NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(previousType);
        }

        public void ChangeStateBySystem(TurnStateType newType)
        {
            currentState?.CompleteState();
            SetNewState(_stateHolder.GetStateByType(newType));
        }

        public void SetNewState(BaseTurnState newState)
        {
            previousState = currentState;
            currentState = newState;

            currentState.SetTeams(turnHelper.TurnTeams);
            currentState.EnterState();
            turnHelper.GetPreviousStateData(previousState, currentState);
        }

        public void EndTurn()
        {
            if (turnHelper.GameEnding()) return;

            TurnStatusEvents.OnTurnEnding?.Invoke();
            turnHelper.SwitchTeams();
            NewTurn();

            foreach (var state in _stateHolder.States)
            {
                var turnData = (ITransferDataHolder<BaseTurnTransferData>) state;
                turnData.TransferData.ResetPreviousTurnData();
            }
        }

        private void OnDisable()
        {
            Eventbus.TeamEvents.OnTeamsSet -= SetTurnTeams;

            bpEventHandler.UnsubscribeFromBlueprintEvents();

            NetworkEventbus.OnAllClientsSet -= FirstTurn;
            NetworkEventbus.RequestEvents.OnStateChangeRequestByServer -= ChangeStateBySystem;

            Eventbus.CombatEvents.OnCombatTerminated -= EndTurn; //TODO: check
            UIEventbus.OnButtonCall -= ShowButtonRequest;
            UIEventbus.OnButtonClicked -= StateChangeRequestByUser;
            Eventbus.StateEvents.OnStateChangeWithoutInteraction -= StateChangeRequestByUser;
        }

       
    }
}