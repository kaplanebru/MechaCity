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
        public static Team[] Teams;
        public TeamsHolder assetHolder;
        public bool isMultiplayerOn = true; //for testing

        private void OnEnable()
        {
            NetworkEventbus.RequestEvents.OnPlayerSpawned += AssignPlayers;
            TowerEvents.OnTowersCreated += CreateTeams;
        }

        void CreateTeams()
        {
            Teams = new Team[assetHolder.Teams.Length];
            for (int i = 0; i < Teams.Length; i++)
            {
                Teams[i] = Instantiate(assetHolder.Teams[i], transform);
                Teams[i].Initialize();
            }

            NetworkUIController.gameObject.SetActive(true);

            Eventbus.TeamEvents.OnTeamsSet?.Invoke(Teams);
        }


        private void AssignPlayers(Player newPlayer, ulong id)
        {
            Teams[id].Data.Player = newPlayer;
            newPlayer.Setup(Teams[id].Data.TeamTowerData.TeamType);

            if (!isMultiplayerOn)
                goto startGame;


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