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
        public static Team[] Teams;
        public TeamsHolder assetHolder;
        public ActorHolder ActorHolder = new();

        private void OnEnable()
        {
            //GeneralEventbus.InitializerEvents.OnInitialize?.Invoke();
            NetworkEventbus.ServerEvents.OnPlayerSpawned += AssignPlayers;
            GeneralEventbus.InitializerEvents.OnTowersCreated += ExecuteInitializer;
            ActorHolder.Initialize();
        }

        void ExecuteInitializer()
        {
            Invoke(nameof(CreateTeams), 1);
        }

        void CreateTeams()
        {
            Teams = new Team[assetHolder.Teams.Length];
           
            for (int i = 0; i < Teams.Length; i++)
            {
                Teams[i] = Instantiate(assetHolder.Teams[i], transform);
                Teams[i].SetTowers();
            }
            
            NetworkUIController.gameObject.SetActive(true);

            TeamEvents.OnTeamsSet?.Invoke(Teams);
            GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet?.Invoke();
            
            Invoke(nameof(TowerAndTeamsReadyCall), 0.5f);
        }

        void TowerAndTeamsReadyCall() //todo: temp
        {
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady?.Invoke();
        }


        private void AssignPlayers(Player newPlayer, ulong id)
        {
            Teams[id].Data.Player = newPlayer;
            newPlayer.Setup(Teams[id].Data.TeamTowerData.TeamType);
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
            
            foreach (var t in AllTowers.TowerDatas)
            {
                //t.Mover.ChangeHeight(t.Height, true); //TODO: OPEN
                if(t.LockStatus.Locked)
                    Eventbus.TowerEvents.OnLock?.Invoke(t.LockStatus.Limit, t.UniqID);
            }


            print("Game Started");
        }


        private void OnDisable()
        {
            NetworkEventbus.ServerEvents.OnPlayerSpawned -= AssignPlayers;
            GeneralEventbus.InitializerEvents.OnTowersCreated -= ExecuteInitializer;
            ActorHolder.Unsubscribe();
        }
    }
}