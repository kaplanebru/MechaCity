using System.Linq;
using Enums;
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
            Eventbus.TeamEvents.OnTeamsSet += GetTeams;
            Eventbus.CombatEvents.OnTowerKilled += ExchangeTower;
        }

        public void GetTeams(Team[] teams)
        {
            _teams = teams;
        }
    
         Team GetTeamDataByTeamType(TeamType type) => _teams.First(team => team.Data.TeamType == type);

         private TowerData _deadTower;
         private void ExchangeTower(TowerData deadTower)
         {
             _deadTower = deadTower;
            Team oldTeam = GetTeamDataByTeamType(deadTower.TeamTowerData.TeamType);
            Team newTeam = _teams.FirstOrDefault(t => t != oldTeam);

            oldTeam.RemoveTower(deadTower);
            newTeam.TakeTowerFromRival(deadTower);
            
            
            Invoke(nameof(ResetDeadTower), 1f); //todo: temporary
        }

         void ResetDeadTower()
         {
             AllTowers.GetTower(_deadTower.UniqID).ResetHealth();
         }

        private void OnDisable()
        {
            Eventbus.TeamEvents.OnTeamsSet -= GetTeams;
            Eventbus.CombatEvents.OnTowerKilled -= ExchangeTower;
        }
    }
}
