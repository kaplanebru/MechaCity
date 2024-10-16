using System.Collections.Generic;
using System.Linq;
using Actor;
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
            Eventbus.CombatEvents.OnActorKilled += ExchangeTowers;
        }

        public void GetTeams(Team[] teams)
        {
            _teams = teams;
        }
    
         Team GetTeamDataByTeamType(TeamType type) => _teams.First(team => team.Data.TeamType == type);

         private uint _deadActorID;
         private void ExchangeTowers(uint actorID)
         {
             _deadActorID = actorID;
             var actor = ActorHolder.Registry[actorID];
             foreach (var deadTower in actor.Towers)
             {
                 ExchangeTower(deadTower);
             }
             
             Invoke(nameof(ResetHealth), 1f); //todo: temporary
         }
         private void ExchangeTower(TowerData deadTower)
         {
             Team oldTeam = GetTeamDataByTeamType(deadTower.TeamType);
            Team newTeam = _teams.FirstOrDefault(t => t != oldTeam);

            oldTeam.RemoveTower(deadTower);
            newTeam.TakeTowerFromRival(deadTower);
         }

         void ResetHealth()
         {
             Eventbus.CombatEvents.OnTeamSwitch?.Invoke(_deadActorID);
         }

        private void OnDisable()
        {
            TeamEvents.OnTeamsSet -= GetTeams;
            Eventbus.CombatEvents.OnActorKilled -= ExchangeTowers;
        }
    }
}
