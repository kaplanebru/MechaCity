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


namespace Turn
{
    public class TurnManager : MonoBehaviour ////NetworkBehaviour
    {
        private BaseTurnState currentState;

        private SelectionState selectionState = new SelectionState();
        private TowerGroupState towerGroupState = new TowerGroupState();
        private CombatState combatState = new CombatState();
        private ExitState exitState = new ExitState();

        BaseTurnState[] states = new BaseTurnState[3];
        Dictionary<string, Team> turnTeams;

        public TeamType currentTeamType = TeamType.Team1;
        public CombatTimingData timingData;


        private void OnEnable()
        {
            Eventbus.TeamEvents.OnTeamsSet += SetTurnTeams;
            SubscribeToBlueprintActions();

            NetworkEventbus.OnAllClientsSet += FirstTurn;
            NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer += CompleteStateBySystem;
            NetworkEventbus.RequestEvents.OnNewTurnRequest += NewTurn;
        }

        private void Initialize()
        {
            Setup();
            UIEventbus.TurnEvents.OnInitialize?.Invoke();
        }

        void SubscribeToBlueprintActions()
        {
            Eventbus.BlueprintEvents.OnReverseOrderActionBegin += PublishReverseOrderAction;
        }

        void UnsubscribeToBlueprintActions()
        {
            Eventbus.BlueprintEvents.OnReverseOrderActionBegin -= PublishReverseOrderAction;
        }

        void PublishReverseOrderAction()
        {
            //Combata event publish edecek.
            //selection ve group aç-kapa şeklinde çalışabilir. Ya da state machine yapılır ya. Combat hep açık olabilir.
            //BP actionları için de action olarak kart oluşturmaya bak
        }

        void SetTurnTeams(Team[] teams)
        {
            turnTeams = new Dictionary<string, Team>()
            {
                {"currentTeam", teams[0]},
                {"rivalTeam", teams[1]},
            };
        }

        void Setup()
        {
            states[0] = selectionState;
            states[1] = towerGroupState;
            states[2] = combatState;
            //states[3] = exitState;

            for (int i = 0; i < states.Length; i++)
            {
                states[i].StateId = i;
            }
        }

        void FirstTurn(params object[] args)
        {
            Initialize();
            SetFirstCombatElements();
        }

        void SetFirstCombatElements()
        {
            combatState.ConstantSetup();
            combatState.CompleteState();

            NetworkEventbus.TurnEvents.OnTurnStarted?.Invoke(currentTeamType);

            ConstructCurrentState(states[0]);
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

            var transferData = ((ITurnTransferHandler<BaseTurnTransferData>) states[turnIndex - 1]).TransferData;
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
            if(newState == null) return;
            ConstructCurrentState(newState);
        }

        [CanBeNull]
        BaseTurnState GetNextState(int nextStateID)
        {
            if (nextStateID == states.Length)
            {
                if (!GameEnding())
                {
                    foreach (var state in states)
                    {
                        state.ResetPreviousTurnData();
                    }

                    //NetworkEventbus.TurnEvents.OnTurnEnding?.Invoke();
                    NetworkEventbus.TriggerEvents.OnCompleteStateRequestByUser?.Invoke(currentState.StateType);
                }

                return null; //states[0];
            }

            return states[nextStateID];
        }

        void NewTurn()
        {
            // StopCoroutine(nameof(TurnActionRoutine));
            // SwitchTeams();
            // StartCoroutine(nameof(TurnActionRoutine));
            SwitchTeams();
            NetworkEventbus.TurnEvents.OnTurnStarted?.Invoke(currentTeamType);

            ConstructCurrentState(states[0]);
        }

        void SwitchTeams()
        {
            currentTeamType = turnTeams["rivalTeam"].Data.TeamType;
            (turnTeams["currentTeam"], turnTeams["rivalTeam"]) = (turnTeams["rivalTeam"], turnTeams["currentTeam"]);

            UIEventbus.OnTeamSwitch?.Invoke(currentTeamType);
        }

        bool GameEnding()
        {
            foreach (var team in turnTeams)
            {
                if (team.Value.Data.Towers.Count < 2 || team.Value.Data.Towers.All(t => t.Health == 0))
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
            UnsubscribeToBlueprintActions();

            NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer -= CompleteStateBySystem;
            NetworkEventbus.RequestEvents.OnNewTurnRequest -= NewTurn;
            NetworkEventbus.OnAllClientsSet -= FirstTurn;
        }
    }
}