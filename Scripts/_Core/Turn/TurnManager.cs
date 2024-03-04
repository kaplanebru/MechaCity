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
using Towers;


namespace Turn
{
    public class TurnManager : MonoBehaviour ////NetworkBehaviour
    {
        public static bool IsMultiplayerOn = true; //for testing
        public static int TurnTracker => _turnTracker;
        private static int _turnTracker = 0;

        private BaseTurnState currentState;
        private BaseTurnState previousState;
        public TurnStateHolder stateHolder = new TurnStateHolder();
        private IntruderState intruderState; // = new StateIntruder();

        Dictionary<TeamState, Team> turnTeams;

        public TeamType currentTeamType = TeamType.Team1;
        public CombatTimingData timingData; //TODO: Turn asset holder
        private CombatHelper _combatHelper;


        private void OnEnable()
        {
            Eventbus.TeamEvents.OnTeamsSet += SetTurnTeams;

            NetworkEventbus.BlueprintEvents.OnStateIntrusionAttempt += ActivateStateIntruder;
            NetworkEventbus.BlueprintEvents.OnStateIntrusionEnd += DeActivateIntrusion;
            SubscribeToBlueprintActions();

            NetworkEventbus.OnAllClientsSet += FirstTurn;
            NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer += ChangeStateBySystem;
            NetworkEventbus.RequestEvents.OnNewTurnRequest += SetNewTurn;
        }

        private void Initialize()
        {
            stateHolder.Setup();
            intruderState = (IntruderState) stateHolder.StatesByType[TurnStateType.Intruder];

            UIEventbus.TurnEvents.OnInitialize?.Invoke();
        }

        void SubscribeToBlueprintActions()
        {
            BpEventbus.ActionEvents.OnReverseActionTriggered += PublishReverseOrderAction;
        }

        void UnsubscribeFromBlueprintActions()
        {
            BpEventbus.ActionEvents.OnReverseActionTriggered -= PublishReverseOrderAction;
        }

        void PublishReverseOrderAction()
        {
            BpEventbus.SubscriberEvents.OnReverseAction?.Invoke();
        }

        private void ActivateStateIntruder()
        {
            NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser.Invoke(TurnStateType.Intruder);
            print("Activate");
        }

        private void DeActivateIntrusion()
        {
            intruderState.StopIntrusion();
        }

        void SetTurnTeams(Team[] teams)
        {
            turnTeams = new Dictionary<TeamState, Team>()
            {
                {TeamState.CurrentTeam, teams[0]},
                {TeamState.RivalTeam, teams[1]},
            };
        }

        private bool firstTurn = true;

        void FirstTurn(params object[] args)
        {
            Initialize();
            _combatHelper = ((ExitState) stateHolder.StatesByType[TurnStateType.Exit]).combatHelper;
            _combatHelper.Subscribe(null, this);
            _combatHelper.SetCombatPairs();

            NewTurn();
            firstTurn = false;
        }

        void NewTurn()
        {
            _turnTracker++;
            NetworkEventbus.TurnEvents.OnTurnStarted?.Invoke(currentTeamType);

            if (firstTurn)
                SetNewState(stateHolder.StatesByType[TurnStateType.Selection]);
            else
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(TurnStateType.Selection);
        }

        public void StateChangeRequestByUser()
        {
            NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(GetNextStateType());
        }

        public void ChangeStateBySystem(TurnStateType newType)
        {
            currentState.CompleteState();
            SetNewState(stateHolder.StatesByType[newType]);
        }

        public void SetNewState(BaseTurnState newState)
        {
            previousState = currentState;
            currentState = newState;
            
            currentState.SetTeams(turnTeams);
            currentState.EnterState(this); //TODO: Her defasında turn managerı göndermesi saçma
            GetPreviousStateData();
        }

        void GetPreviousStateData()
        {
            if (previousState == null) return;

            var previousTransferData = ((ITurnTransferHandler<BaseTurnTransferData>) previousState).TransferData;
            currentState.ProcessPreviousStateTransferData(previousTransferData);
        }

        TurnStateType GetNextStateType()
        {
            var nextStateId = (currentState.StateId + 1) % (stateHolder.States.Length - 1);
            return stateHolder.States[nextStateId].StateType;
        }

        void SetNewTurn()
        {
            SwitchTeams();
            NewTurn();
        }

        void SwitchTeams()
        {
            currentTeamType = turnTeams[TeamState.RivalTeam].Data.TeamType;
            (turnTeams[TeamState.CurrentTeam], turnTeams[TeamState.RivalTeam]) =
                (turnTeams[TeamState.RivalTeam], turnTeams[TeamState.CurrentTeam]);

            UIEventbus.OnTeamSwitch?.Invoke(currentTeamType);
        }

        private void OnDisable()
        {
            Eventbus.TeamEvents.OnTeamsSet -= SetTurnTeams;

            NetworkEventbus.BlueprintEvents.OnStateIntrusionAttempt -= ActivateStateIntruder;
            NetworkEventbus.BlueprintEvents.OnStateIntrusionEnd -= DeActivateIntrusion;
            UnsubscribeFromBlueprintActions();

            NetworkEventbus.OnAllClientsSet -= FirstTurn;
            NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer -= ChangeStateBySystem;
            NetworkEventbus.RequestEvents.OnNewTurnRequest -= SetNewTurn;
        }

        public void EndTurn()
        {
            if(GameEnding()) return;
            NetworkEventbus.RequestEvents.OnNewTurnRequest?.Invoke();
            
            foreach (var state in stateHolder.States)
            {
                var turnData = (ITurnTransferHandler<BaseTurnTransferData>) state;
                turnData.TransferData.ResetPreviousTurnData();
            }
        }

        bool GameEnding()
        {
            foreach (var team in turnTeams)
            {
                if (team.Value.Data.Towers.Count < 2 || team.Value.Data.Towers.All(t => t.Health == 0)) //TODO: CHECK
                {
                    NetworkEventbus.TriggerEvents.OnGameEnds?.Invoke(team.Value.Data.TeamType);
                    print("game ends");
                    return true;
                }
            }
            return false;
        }
    }
}