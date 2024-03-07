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
        private IntruderState intruderState;
        private BlueprintEventHandler bpEventHandler;

        Dictionary<TeamState, Team> turnTeams;
        public TeamType currentTeamType = TeamType.Team1;

        private CombatHelper _combatHelper;

        private bool firstTurn = true;


        private void OnEnable()
        {
            Eventbus.TeamEvents.OnTeamsSet += SetTurnTeams;

            NetworkEventbus.BlueprintEvents.OnStateIntrusionEnd += DeActivateIntrusion;
            NetworkEventbus.OnAllClientsSet += FirstTurn;
            NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer += ChangeStateBySystem;
            Eventbus.CombatEvents.OnCombatTerminated += EndTurn;
            
            
            UIEventbus.OnButtonCall += ShowButtonRequest; //todo: sadece state'i tutan bir kod olabilir, state'e göre action alan
            UIEventbus.OnButtonClicked += HandleClick;
            
            bpEventHandler = new BlueprintEventHandler(this);
        }

        private void HandleClick()
        {
            if(currentState != intruderState)
                StateChangeRequestByUser();
            else
            {
                print("blueprint");
                //bpmanagera network eventi
            }
        }

        private void ShowButtonRequest(bool enable)
        {
            UIEventbus.OnShowButtonRequest?.Invoke(enable, currentState.StateType);
        }


        private void DeActivateIntrusion()
        {
            intruderState.StopIntrusion();
        }

        private void Initialize()
        {
            _stateHolder.Setup();
            intruderState = (IntruderState) _stateHolder.GetStateByType(TurnStateType.Intruder);

            UIEventbus.TurnEvents.OnInitialize?.Invoke();
        }

        void SetTurnTeams(Team[] teams)
        {
            turnTeams = new Dictionary<TeamState, Team>()
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
            ManageInput();
            SetFirstState();
        }

        void SetFirstState()
        {
            if (firstTurn)
                SetNewState(
                    _stateHolder.GetStateByType(TurnStateType
                        .Selection)); //eğer first turn ise hala network döngüsündeyiz
            else
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(TurnStateType.Selection);
        }

        public void StateChangeRequestByUser()
        {
            NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(GetNextStateType());
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

            currentState.SetTeams(turnTeams);
            currentState.EnterState();
            GetPreviousStateData();
        }

        public void EndTurn()
        {
            if (GameEnding()) return;

            SwitchTeams();
            NewTurn();

            foreach (var state in _stateHolder.States)
            {
                var turnData = (ITransferDataHolder<BaseTurnTransferData>) state;
                turnData.TransferData.ResetPreviousTurnData();
            }
        }


        /// /////////////
        void GetPreviousStateData()
        {
            if (previousState == null) return;

            var previousTransferData = ((ITransferDataHolder<BaseTurnTransferData>) previousState).TransferData;
            currentState.ProcessPreviousStateTransferData(previousTransferData);
        }

        TurnStateType GetNextStateType()
        {
            var nextStateId = (currentState.StateId + 1) % (_stateHolder.States.Length - 1);
            return _stateHolder.States[nextStateId].StateType;
        }

        void SwitchTeams()
        {
            currentTeamType = turnTeams[TeamState.RivalTeam].Data.TeamType;
            (turnTeams[TeamState.CurrentTeam], turnTeams[TeamState.RivalTeam]) =
                (turnTeams[TeamState.RivalTeam], turnTeams[TeamState.CurrentTeam]);

            UIEventbus.OnTeamSwitch?.Invoke(currentTeamType);
        }

        void ManageInput()
        {
            if (!MultiplayerSetter.IsMultiplayerOn) return;
            turnTeams[TeamState.CurrentTeam].Data.Player.EnableInput(true);
            turnTeams[TeamState.RivalTeam].Data.Player.EnableInput(false);
        }

        private void OnDisable()
        {
            Eventbus.TeamEvents.OnTeamsSet -= SetTurnTeams;

            NetworkEventbus.BlueprintEvents.OnStateIntrusionEnd -= DeActivateIntrusion;
            bpEventHandler.UnsubscribeFromBlueprintEvents();

            NetworkEventbus.OnAllClientsSet -= FirstTurn;
            NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer -= ChangeStateBySystem;

            Eventbus.CombatEvents.OnCombatTerminated -= EndTurn; //TODO: check
            UIEventbus.OnButtonCall -= ShowButtonRequest;
            UIEventbus.OnButtonClicked -= HandleClick;

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