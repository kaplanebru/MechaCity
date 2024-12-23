using System.Collections.Generic;
using Actor;
using UnityEngine;
using Enums;
using GameUI;
using Health;
using Network;
using PlayerNetwork;
using Teams;
using Testing;
using Towers;


namespace Core
{
    public class PlayerAndTeamInitializer : MonoBehaviour
    {
        public Transform NetworkUIController;
        public Team[] Teams;
        public TeamData[] TeamsData;
       
        
        private void OnEnable()
        {
            NetworkEventbus.ServerEvents.OnPlayerSpawned += AssignPlayers;
            GeneralEventbus.InitializerEvents.OnActorsAndTowersReady += ExecuteInitializer;
        }
        
        void ExecuteInitializer()
        {
            CreateTeams();
            Invoke(nameof(StartNetwork), .6f);
        }

        void CreateTeams()
        {
            Teams = new Team[TeamsData.Length];
            for (int i = 0; i < Teams.Length; i++)
            {
                Teams[i] = new Team(TeamsData[i]);
                Teams[i].DistributeTeamActors();
            }
            
            TeamEvents.OnTeamsSet?.Invoke(Teams);
        }
        
        private void StartNetwork()
        {
            NetworkUIController.gameObject.SetActive(true);
        }
        private void AssignPlayers(Player newPlayer, ulong id)
        {
            Teams[id].Data.Player = newPlayer;
            newPlayer.Setup(Teams[id].Data.teamColorData.TeamType);
            UIEventbus.OnPlayerSet?.Invoke(Teams[id].Data.Name);

            if (!MultiplayerSetter.IsMultiplayerOn)
            {
                newPlayer.EnableInput(true);
                goto startGame;
            }
            
            foreach (var team in Teams)
            {
                if (team.Data.Player == null)
                {
                    print("Waiting for other players to join..."); //sadece client1'de görünmeli
                    return;
                }
            }

            startGame:

            NetworkEventbus.OnAllClientsSet?.Invoke(new object[]
                {
                    new Dictionary<TeamType, string>
                    {
                        {Teams[0].Data.TeamType, Teams[0].Data.Name},
                        {Teams[1].Data.TeamType, Teams[1].Data.Name},
                    }
                }
            );
            
            GeneralEventbus.InitializerEvents.OnTeamsAndClientsSet?.Invoke();
            
            print("Game Started");
        }


        private void OnDisable()
        {
            NetworkEventbus.ServerEvents.OnPlayerSpawned -= AssignPlayers;
            GeneralEventbus.InitializerEvents.OnActorsAndTowersReady -= ExecuteInitializer;
        }
    }
}