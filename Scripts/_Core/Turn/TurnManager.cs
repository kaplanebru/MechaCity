using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Network;
using Unity.Netcode;
using UnityEngine;
using Teams;
using Towers;
using UI;

namespace Turn
{
    public class TurnManager : MonoBehaviour ////NetworkBehaviour
    {
        public NetworkVariable<TurnHandlerType> turnHandlerType = new(TurnHandlerType.Selection);
        BaseTurnHandler[] turnHandlers;
        Dictionary<string, Team> turnTeams;

        
        public TeamType currentTeamType = TeamType.Team1;

        private BaseTurnHandler currentTurnHandler;


        private void OnEnable()
        {
             Eventbus.TeamEvents.OnTeamsSet += SetTurnTeams;
            
            NetworkEventbus.OnAllClientsSet += FirstTurn;
            NetworkEventbus.RequestEvents.OnCompleteActionRequest += CompleteActionByUser;
            NetworkEventbus.RequestEvents.OnNewTurnRequest += NewTurn;

            turnHandlers = GetComponentsInChildren<BaseTurnHandler>(true).ToArray();
            DisableAllTurnHandlers();
        }

        private void Initialize()
        {
            UIEventbus.TurnEvents.OnInitialize?.Invoke();
        }


        void SetTurnTeams(Team[] teams)
        {
            turnTeams = new Dictionary<string, Team>()
            {
                {"currentTeam", teams[0]},
                {"rivalTeam", teams[1]},
            };
        }

        void DisableAllTurnHandlers()
        {
            foreach (var turnHandler in turnHandlers)
            {
                turnHandler.enabled = false;
            }
        }


        void FirstTurn(params object[] args)
        {
            // var x = args[0] as Team[];
            // foreach (var team in x)
            // {
            //     print(team.name);
            // }
            // SetTurnTeams(args[0] as Team[]);
           
            Initialize();
            SetFirstCombatElements();

            StartCoroutine(nameof(TurnActionRoutine));
        }

        void SetFirstCombatElements()
        {
            var combatHandler = turnHandlers.FirstOrDefault(i =>
                    i as CombatHandler != null) as CombatHandler;
            combatHandler.enabled = true;
            foreach (var tower in turnTeams["currentTeam"].Data.TowerIds)
            {
                combatHandler.CreateCombatPairByTower(AllTowers.GetTower(tower));
            }

            // var teamSwitcher = combatHandler.TurnHelpers.FirstOrDefault(h => h as TeamSwitcher != null) as TeamSwitcher;
            // teamSwitcher.GetTeams(initializer.teams);
            combatHandler.CompleteAction();
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
                if (team.Value.Data.TowerIds.Count < 2 || team.Value.Data.TowerIds.All(t => AllTowers.GetTower(t).Data.Health == 0))
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
            
            NetworkEventbus.RequestEvents.OnCompleteActionRequest -= CompleteActionByUser;
            NetworkEventbus.RequestEvents.OnNewTurnRequest -= NewTurn;
            NetworkEventbus.OnAllClientsSet -= FirstTurn;
        }
    }
}