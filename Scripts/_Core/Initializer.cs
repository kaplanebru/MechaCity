using System.Collections.Generic;
using Data;
using UnityEngine;
using Enums;
using Network;
using PlayerNetwork;
using Teams;

namespace Core
{
    public class Initializer : MonoBehaviour
    {
        public Team[] teams;
        public TeamsHolder assetHolder;
        public bool isMultiplayerOn = true; //for testing

        private void OnEnable()
        {
            NetworkEventbus.RequestEvents.OnPlayerSpawned += SetPlayerForTeam;
            CreateTeams();
            TurnOffMultiplayer();
            
        }

        void TurnOffMultiplayer()
        {
            if (!isMultiplayerOn)
            {
                print("Multiplayer features are off");
                Eventbus.TeamEvents.OnTeamsSet?.Invoke(teams);
                NetworkEventbus.OnAllClientsSet?.Invoke(null);
                return;
            }
        }
        
        void CreateTeams()
        {
            teams = new Team[assetHolder.Teams.Length];
            for (int i = 0; i < teams.Length; i++)
            {
                teams[i] = Instantiate(assetHolder.Teams[i], transform);
                teams[i].Initialize();
            }

            SetFirstMatches();
        }

        void SetFirstMatches() //Temporary
        {
            teams[0].LinkFirstMatches(teams[1]);
            teams[1].LinkFirstMatches(teams[0]);
           
        }


        private void SetPlayerForTeam(Player player, ulong id)
        {
            teams[id].Data.Player = player;
            player.Setup(teams[id].Data.TeamTowerData.TeamType);

            foreach (var team in teams)
            {
                if (team.Data.Player == null)
                {
                    print("Waiting for other players to join..."); //sadece client1'de görünmeli
                    return;
                }
            }

            Eventbus.TeamEvents.OnTeamsSet?.Invoke(teams);
            NetworkEventbus.OnAllClientsSet?.Invoke(new object[]
                {
                    new Dictionary<TeamType, string>
                    {
                        {teams[0].Data.TeamType, teams[0].Data.Name},
                        {teams[1].Data.TeamType, teams[1].Data.Name},
                    }
                }
            );
            print("Game Started");
        }

        private void OnDisable()
        {
            NetworkEventbus.RequestEvents.OnPlayerSpawned -= SetPlayerForTeam;
        }
    }
}