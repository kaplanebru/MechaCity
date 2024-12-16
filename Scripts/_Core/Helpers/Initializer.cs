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
    public class Initializer : MonoBehaviour
    {
        public Transform NetworkUIController;
        public Team[] Teams;
        public TeamData[] TeamsData;
        [SerializeField] Transform levelPrefab;
     
        
        private void OnEnable()
        {
            NetworkEventbus.ServerEvents.OnPlayerSpawned += AssignPlayers;
            GeneralEventbus.InitializerEvents.OnActorsCreated += ExecuteInitializer;
          
            InstantiateLevelPrefab();
        }
        
        void InstantiateLevelPrefab()
        {
            Instantiate(levelPrefab, transform);
        }

        void ExecuteInitializer()
        {
            Invoke(nameof(CreateTeams), 1);
        }

        void CreateTeams()
        {
            Teams = new Team[TeamsData.Length];
           
            for (int i = 0; i < Teams.Length; i++)
            {
                Teams[i] = new Team(TeamsData[i]);
                Teams[i].SetTeamActors();
            }
            
            NetworkUIController.gameObject.SetActive(true);

            TeamEvents.OnTeamsSet?.Invoke(Teams);
            GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet?.Invoke();
            
            Invoke(nameof(TowerAndTeamsReadyCall), 1.5f); //todo: later, henüz extralar eklenmemişse olmaz
        }

        void TowerAndTeamsReadyCall() //todo: temp
        {
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady?.Invoke();
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
            
           
            
            ExecuteAfterSetup();


            print("Game Started");
        }

        public void ExecuteAfterSetup()
        {
            foreach (var tower in AllTowers.Towers)
            {
                var data = tower.Data;
                if(data.LockStatus.Locked)
                    Eventbus.TowerEvents.OnLock?.Invoke(data.LockStatus.Limit, data.UniqID);
                
                tower.initializer.ExecuteAfterSetup();
            }
        }

        private void OnDisable()
        {
            NetworkEventbus.ServerEvents.OnPlayerSpawned -= AssignPlayers;
            GeneralEventbus.InitializerEvents.OnActorsCreated -= ExecuteInitializer;
        }
    }
}