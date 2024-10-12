using System.Collections.Generic;
using System.Linq;
using Enums;
using Health;
using Teams;
using Towers;
using UnityEngine;

namespace Turn
{
    public class TeamSwitcher : BaseTurnHelper
    {
        [SerializeField] Team[] _teams; //turnmanagerdan da alınabilir
      

        private void OnEnable()
        {
            TeamEvents.OnTeamsSet += GetTeams;
            Eventbus.CombatEvents.OnTowerKilled += ExchangeTowers;
        }

        public void GetTeams(Team[] teams)
        {
            _teams = teams;
        }
    
         Team GetTeamDataByTeamType(TeamType type) => _teams.First(team => team.Data.TeamType == type);

         private void ExchangeTowers(List<int> towers)
         {
             foreach (var deadTowerId in towers)
             {
                 ExchangeTower(deadTowerId);
             }
         }
         private void ExchangeTower(int deadTowerId)
         {
            var deadTower = AllTowers.GetData(deadTowerId);
            Team oldTeam = GetTeamDataByTeamType(deadTower.TeamType);
            Team newTeam = _teams.FirstOrDefault(t => t != oldTeam);

            oldTeam.RemoveTower(deadTower);
            newTeam.TakeTowerFromRival(deadTower);
            
            
            Invoke(nameof(ResetDeadTower), 1f); //todo: temporary
        }

         void ResetDeadTower()
         {
             //HealthHandler.ResetHealth(_deadTower.UniqID);
         }

        private void OnDisable()
        {
            TeamEvents.OnTeamsSet -= GetTeams;
            Eventbus.CombatEvents.OnTowerKilled -= ExchangeTowers;
        }
    }
}
