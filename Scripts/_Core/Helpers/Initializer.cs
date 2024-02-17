using System.Collections.Generic;
using UnityEngine;
using Enums;
using Network;
using PlayerNetwork;
using Teams;
using Towers;



namespace Core
{
    public class Initializer : MonoBehaviour
    {
        public Transform NetworkUIController;
        public Team[] teams;
        public TeamsHolder assetHolder;
        public bool isMultiplayerOn = true; //for testing

        private void OnEnable()
        {
            NetworkEventbus.RequestEvents.OnPlayerSpawned += AssignPlayers;
            TowerEvents.OnTowersCreated += CreateTeams;
            
        }
        void CreateTeams()
        {
            teams = new Team[assetHolder.Teams.Length];
            for (int i = 0; i < teams.Length; i++)
            {
                teams[i] = Instantiate(assetHolder.Teams[i], transform);
                teams[i].Initialize();
            }
            
            NetworkUIController.gameObject.SetActive(true);
            
            Eventbus.TeamEvents.OnTeamsSet?.Invoke(teams);
        }
        

        private void AssignPlayers(Player newPlayer, ulong id)
        {
            teams[id].Data.Player = newPlayer;
            newPlayer.Setup(teams[id].Data.TeamTowerData.TeamType);

            if (!isMultiplayerOn)
                goto startGame;


            foreach (var team in teams)
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
                        {teams[0].Data.TeamType, teams[0].Data.Name},
                        {teams[1].Data.TeamType, teams[1].Data.Name},
                    }
                }
            );
            
            //AllTowers.EveryTower.ForEach(t=>t.towerParts.ChangeHeight(t.Data.Height));
            foreach (var t in AllTowers.Towers)
            {
                t.towerParts.ChangeHeight(t.Data.Height);
            }

            print("Game Started");
        }
        

        private void OnDisable()
        {
            NetworkEventbus.RequestEvents.OnPlayerSpawned -= AssignPlayers;
            TowerEvents.OnTowersCreated -= CreateTeams;
        }
    }
}
