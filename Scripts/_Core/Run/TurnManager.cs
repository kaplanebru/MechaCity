using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Network;
using Unity.Netcode;
using UnityEngine;
using Teams;
using UI;

namespace Core
{
    public class TurnManager : MonoBehaviour ////NetworkBehaviour
    {
        public NetworkVariable<TurnHandlerType> turnHandlerType = new(TurnHandlerType.Selection);
        BaseTurnHandler[] turnHandlers;
        Dictionary<string, Team> turnTeams;
        
        [SerializeField] private TeamsHandler teamsHandler;
        public TeamType currentTeamType = TeamType.Team1;

        private BaseTurnHandler currentTurnHandler;


        private void OnEnable()
        {
            NetworkEventbus.OnAllClientsSet += FirstTurn;
            NetworkEventbus.RequestEvents.OnCompleteActionRequest += CompleteActionByUser;
            NetworkEventbus.RequestEvents.OnNewTurnRequest += NewTurn;

            turnHandlers = GetComponentsInChildren<BaseTurnHandler>(true).ToArray();
            DisableAllTurnHandlers();
        }

        private void Initialize()
        {
            SetTurnTeams();
            UIEventbus.TurnEvents.OnInitialize?.Invoke();
        }


        void SetTurnTeams()
        {
            turnTeams = new Dictionary<string, Team>()
            {
                {"currentTeam", teamsHandler.teams[0]},
                {"rivalTeam", teamsHandler.teams[1]},
            };
        }

        void DisableAllTurnHandlers()
        {
            foreach (var turnHandler in turnHandlers)
            {
                turnHandler.enabled = false;
            }
        }


        public void FirstTurn(params object[] args)
        {
            Initialize();
            var combatHandler = turnHandlers.Last() as CombatHandler;
            foreach (var tower in turnTeams["currentTeam"].Data.Towers)
            {
                combatHandler.CreateCombatPairByTower(tower);
            }
            //combatHandler.RestoreBullets();
            combatHandler.CompleteAction();
            
            StartCoroutine(nameof(TurnActionRoutine));
        }

        IEnumerator TurnActionRoutine()
        {
            
            
            NetworkEventbus.TurnEvents.OnTurnStarted?.Invoke(currentTeamType);


            for (var i = 0; i < turnHandlers.Length; i++)
            {
                currentTurnHandler = turnHandlers[i];
                currentTurnHandler.enabled = true;
                currentTurnHandler.SetTeams(turnTeams);

                GetIncomingData(i);
                currentTurnHandler.Setup();

                yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed);
            }

            if (!GameEnding())
                NetworkEventbus.TurnEvents.OnTurnEnding?.Invoke();
        }

        void GetIncomingData(int turnIndex)
        {
            if (turnIndex <= 0) return;

            var transferData = ((ITurnActionHandler<BaseTurnTransferData>) turnHandlers[turnIndex - 1]).TransferData;
            currentTurnHandler.ProcessIncomingData(transferData);
        }

        void NewTurn()
        {
            StopCoroutine(nameof(TurnActionRoutine));
            SwitchTeams();
            StartCoroutine(nameof(TurnActionRoutine));
        }

        void CompleteActionByUser()
        {
            currentTurnHandler.CompleteAction();
        }

        void SwitchTeams()
        {
            currentTeamType = turnTeams["rivalTeam"].Data.TeamType;
            (turnTeams["currentTeam"], turnTeams["rivalTeam"]) = (turnTeams["rivalTeam"], turnTeams["currentTeam"]);

            UIEventbus.OnTeamSwitch?.Invoke(currentTeamType);

            // var temp = currentTeam;
            // currentTeam = rivalTeam;
            // rivalTeam = temp;
        }

        bool GameEnding()
        {
            foreach (var team in turnTeams)
            {
                if (team.Value.Data.Towers.Count < 2 || team.Value.Data.Towers.All(t => t.Data.Health == 0))
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
            NetworkEventbus.RequestEvents.OnCompleteActionRequest -= CompleteActionByUser;
            NetworkEventbus.RequestEvents.OnNewTurnRequest -= NewTurn;
            NetworkEventbus.OnAllClientsSet -= FirstTurn;
        }
    }
}