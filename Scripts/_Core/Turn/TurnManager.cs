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
        public static int TurnTracker => _turnTracker;
        private static int _turnTracker = 0;
        
        private BaseTurnState currentState;
        private TurnStateHolder stateHolder = new TurnStateHolder();
        private StateIntruder stateIntruder; // = new StateIntruder();

        Dictionary<TeamState, Team> turnTeams;

        public TeamType currentTeamType = TeamType.Team1;
        public CombatTimingData timingData; //TODO: Turn asset holder
        
        private void OnEnable()
        {
            Eventbus.TeamEvents.OnTeamsSet += SetTurnTeams;
            
            NetworkEventbus.BlueprintEvents.OnStateIntrusionAttempt += ActivateStateIntruder;
            NetworkEventbus.BlueprintEvents.OnStateIntrusionEnd += DeActivateIntrusion;
            SubscribeToBlueprintActions();

            NetworkEventbus.OnAllClientsSet += FirstTurn;
            NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer += CompleteStateBySystem;
            NetworkEventbus.RequestEvents.OnNewTurnRequest += SetNewTurn;
        }
        private void Initialize()
        {
            stateHolder.Setup();
            stateIntruder = new StateIntruder(stateHolder);
            
            
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
            AllTowers.ResetTowerSelectionColors();
            stateIntruder.Activate(currentState.StateId);
        }
        private void DeActivateIntrusion()
        {
            stateIntruder.Unsubscribe();
            
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
            stateHolder.CombatState.Subscribe(); // temp
            stateHolder.CombatState.SetCombatPairs(); //TODO: temp? , event?, bp actionlar tek tek stateleri gerektirebilir?
            NewTurn();
        }
        
        void NewTurn()
        {
            _turnTracker++;
            NetworkEventbus.TurnEvents.OnTurnStarted?.Invoke(currentTeamType);
            ConstructCurrentState(stateHolder.States[0]);
        }

        public void ConstructCurrentState(BaseTurnState newState)
        {
            currentState = newState;

            currentState.SetTeams(turnTeams);
            GetPreviousStateData(currentState.StateId);
            currentState.EnterState(this);
        }

        void GetPreviousStateData(int turnIndex)
        {
            if (turnIndex <= 0) return;

            var transferData = ((ITurnTransferHandler<BaseTurnTransferData>) stateHolder.States[turnIndex - 1]).TransferData;
            currentState.ProcessPreviousStateTransferData(transferData);
        }

        public void CompleteStateRequestByUser()
        {
            NetworkEventbus.TriggerEvents.OnCompleteStateRequestByUser?.Invoke(currentState.StateType);
        }

        void CompleteStateBySystem()
        {
            currentState.CompleteState();
            SwitchState(currentState.StateId + 1);
        }

        public void SwitchState(int nextStateID)
        {
            var newState = GetNextState(nextStateID);
            if (newState == null) return;
            ConstructCurrentState(newState);
        }

        [CanBeNull]
        BaseTurnState GetNextState(int nextStateID)
        {
            if (nextStateID == stateHolder.States.Length) //(nextStateID == stateHolder.CombatState.StateId)
            {
                if (!GameEnding())
                {
                    foreach (var state in stateHolder.States)
                    {
                        var turnData = (ITurnTransferHandler<BaseTurnTransferData>)state;
                        turnData.TransferData.ResetPreviousTurnData();
                    }

                    NetworkEventbus.TriggerEvents.OnCompleteStateRequestByUser?.Invoke(currentState.StateType); //% yaparsak 0'a yani joker state'e gider
                }

                return null;
            }

            return stateHolder.States[nextStateID];
        }

        void SetNewTurn()
        {
            SwitchTeams();
            NewTurn();
        }
        
        void SwitchTeams()
        {
            currentTeamType = turnTeams[TeamState.RivalTeam].Data.TeamType;
            (turnTeams[TeamState.CurrentTeam], turnTeams[TeamState.RivalTeam]) = (turnTeams[TeamState.RivalTeam], turnTeams[TeamState.CurrentTeam]);

            UIEventbus.OnTeamSwitch?.Invoke(currentTeamType);
        }

        bool GameEnding() //TODO: temp?
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

        private void OnDisable()
        {
            Eventbus.TeamEvents.OnTeamsSet -= SetTurnTeams;
            
            NetworkEventbus.BlueprintEvents.OnStateIntrusionAttempt -= ActivateStateIntruder;
            NetworkEventbus.BlueprintEvents.OnStateIntrusionEnd -= DeActivateIntrusion;
            UnsubscribeFromBlueprintActions();

            NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer -= CompleteStateBySystem;
            NetworkEventbus.RequestEvents.OnNewTurnRequest -= SetNewTurn;
            NetworkEventbus.OnAllClientsSet -= FirstTurn;
        }
    }
}