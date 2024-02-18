using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Network;
using Unity.Netcode;
using UnityEngine;
using Teams;
using GameUI;


namespace Turn
{
    public class TurnManager : MonoBehaviour ////NetworkBehaviour
    {
        private BaseTurnState currentState;
        
        private SelectionState selectionState = new SelectionState();
        private TowerGroupState towerGroupState = new TowerGroupState();
        private CombatState combatState = new CombatState();
        private ExitState exitState = new ExitState();
        
        BaseTurnState[] states = new BaseTurnState[4];
        Dictionary<string, Team> turnTeams;
        
        public TeamType currentTeamType = TeamType.Team1;
        

        private void OnEnable()
        {
            Eventbus.TeamEvents.OnTeamsSet += SetTurnTeams;
            SubscribeToBlueprintActions();
            
            NetworkEventbus.OnAllClientsSet += FirstTurn;
            NetworkEventbus.RequestEvents.OnCompleteActionRequest += CompleteActionByUser;
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
            states[3] = exitState;

            for (int i = 0; i < states.Length; i++)
            {
                states[i].StateId = i;
            }
        }

        public void SwitchState(int currentStateId)
        {
            var newState = GetNextState(currentStateId);
            currentState = newState;
            newState.EnterState(this);
        }
        
        BaseTurnState GetNextState(int currentStateId)
        {
            int nextStateId = (currentStateId + 1) % states.Length;
            if (nextStateId == states.Length)
            {
                if (!GameEnding()) //son state'te eklenebilir
                    NetworkEventbus.TurnEvents.OnTurnEnding?.Invoke();
            }
            return states[nextStateId];
        }

      


        void FirstTurn(params object[] args)
        {
            Initialize();
            SetFirstCombatElements();
           // StartCoroutine(nameof(TurnActionRoutine));
        }

        
        void SetFirstCombatElements()
        {
            combatState.ConstantSetup();
            combatState.CompleteAction();
            
            NetworkEventbus.TurnEvents.OnTurnStarted?.Invoke(currentTeamType);
            currentState = selectionState;
            currentState.EnterState(this);
            
            currentState.SetTeams(turnTeams);
            GetIncomingData(currentState.StateId);
            currentState.Setup();
        }

        IEnumerator TurnActionRoutine()
        {
            NetworkEventbus.TurnEvents.OnTurnStarted?.Invoke(currentTeamType);
            
            for (var i = 0; i < states.Length; i++)
            {
                currentState = states[i];
                currentState.SetTeams(turnTeams);
                GetIncomingData(i);
                currentState.Setup();

                yield return new WaitUntil(() => currentState.turnAction == TurnAction.Completed);
            }

            if (!GameEnding()) //son state'te eklenebilir
                NetworkEventbus.TurnEvents.OnTurnEnding?.Invoke();
        }

        void GetIncomingData(int turnIndex)
        {
            if (turnIndex <= 0) return;

            var transferData = ((ITurnActionHandler<BaseTurnTransferData>) states[turnIndex - 1]).TransferData;
            currentState.ProcessIncomingData(transferData);
        }

        void NewTurn()
        {
            StopCoroutine(nameof(TurnActionRoutine));
            SwitchTeams();
            StartCoroutine(nameof(TurnActionRoutine));
        }

        void CompleteActionByUser()
        {
            currentState.CompleteAction();
            SwitchState(currentState.StateId);
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
            
            NetworkEventbus.RequestEvents.OnCompleteActionRequest -= CompleteActionByUser;
            NetworkEventbus.RequestEvents.OnNewTurnRequest -= NewTurn;
            NetworkEventbus.OnAllClientsSet -= FirstTurn;
        }
    }
}