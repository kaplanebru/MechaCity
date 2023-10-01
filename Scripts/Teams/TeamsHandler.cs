using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using System.Linq;
using DataModels;
using PlayerNetwork;
using Towers;

namespace Teams
{
    public class TeamsHandler : MonoBehaviour
    {
        public Team[] teams;
        public TeamsHolder assetHolder;
        public bool isMultiplayerOn = true; //for testing

        private void OnEnable()
        {
            Eventbus.TeamEvents.OnTeamChange += ExchangeTower;
            Eventbus.FireEvents.OnTowerKilled += GetGridByTeam;
            Eventbus.NetworkRequestEvents.OnPlayerSpawned += SetPlayerForTeam;

            CreateTeams();

            TurnOffMultiplayer();
        }

        void TurnOffMultiplayer()
        {
            if (!isMultiplayerOn)
            {
                print("Multiplayer features are off");
                Eventbus.NetworkEvents.OnAllClientsSet?.Invoke(teams);
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

            Eventbus.NetworkEvents.OnAllClientsSet?.Invoke(teams);
            print("Game Started");
        }

        Team GetTeamDataByTeamType(TeamType type) => teams.First(team => team.Data.TeamType == type);

        private void GetGridByTeam(Tower deadTower)
        {
            var team = GetTeamDataByTeamType(deadTower.Data.TeamTowerData.TeamType);
            Eventbus.FireEvents.OnTowerGridDetection?.Invoke(new TowerGridRelationModel(team.Data.Grid, deadTower));
        }

        private void ExchangeTower(Tower deadTower)
        {
            Team oldTeam = GetTeamDataByTeamType(deadTower.Data.TeamTowerData.TeamType);
            Team newTeam = teams.FirstOrDefault(t => t != oldTeam);

            oldTeam.RemoveTower(deadTower);
            newTeam.TakeTowerFromRival(deadTower);
        }

        private void OnDisable()
        {
            Eventbus.TeamEvents.OnTeamChange -= ExchangeTower;
            Eventbus.FireEvents.OnTowerKilled -= GetGridByTeam;
            Eventbus.NetworkRequestEvents.OnPlayerSpawned -= SetPlayerForTeam;
        }
    }
}