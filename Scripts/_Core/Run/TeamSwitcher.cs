using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Enums;
using Teams;
using Towers;
using UnityEngine;

namespace Core
{
    public class TeamSwitcher : BaseTurnHelper
    {
        [SerializeField] Team[] _teams; //turnmanagerdan da alınabilir

        private void OnEnable()
        {
            Eventbus.TeamEvents.OnTeamsSet += GetTeams;
            Eventbus.TeamEvents.OnTeamChange += ExchangeTower;
            Eventbus.CombatEvents.OnTowerKilled += GetGridByTeam;
        }

        public void GetTeams(Team[] teams)
        {
            _teams = teams;
        }
    
        Team GetTeamDataByTeamType(TeamType type) => _teams.First(team => team.Data.TeamType == type);
        private void GetGridByTeam(Tower deadTower)
        {
            var team = GetTeamDataByTeamType(deadTower.Data.TeamTowerData.TeamType);
            Eventbus.CombatEvents.OnTowerGridDetection?.Invoke(new TowerGridRelationModel(team.Data.Grid, deadTower));
        }

        private void ExchangeTower(Tower deadTower)
        {
            Team oldTeam = GetTeamDataByTeamType(deadTower.Data.TeamTowerData.TeamType);
            Team newTeam = _teams.FirstOrDefault(t => t != oldTeam);

            oldTeam.RemoveTower(deadTower);
            newTeam.TakeTowerFromRival(deadTower);
        }

        private void OnDisable()
        {
            Eventbus.TeamEvents.OnTeamsSet -= GetTeams;
            Eventbus.TeamEvents.OnTeamChange -= ExchangeTower;
            Eventbus.CombatEvents.OnTowerKilled -= GetGridByTeam;
        }
    }
}
