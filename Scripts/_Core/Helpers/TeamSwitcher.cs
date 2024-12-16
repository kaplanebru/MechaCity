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
            Eventbus.CombatEvents.OnActorKilled += ExchangeActors;
        }

        public void GetTeams(Team[] teams)
        {
            _teams = teams;
        }

        Team GetTeamDataByTeamType(TeamType type) => _teams.First(team => team.Data.TeamType == type);

        private uint _deadActorID;

        private void ExchangeActors(uint actorID)
        {
            _deadActorID = actorID;
            var actor = ActorHolder.Registry[actorID];
            ExchangeActor(actor);
            
            Invoke(nameof(ResetHealth), 1f); //todo: temporary
        }

        private void ExchangeActor(ActorData deadActor)
        {
            Team oldTeam = GetTeamDataByTeamType(deadActor.TeamType);
            Team newTeam = _teams.FirstOrDefault(t => t != oldTeam);

            oldTeam.RemoveTower(deadActor);
            newTeam.TakeActorFromRival(deadActor);
        }

        void ResetHealth()
        {
            Eventbus.CombatEvents.OnTeamSwitch?.Invoke(_deadActorID);
        }

        private void OnDisable()
        {
            TeamEvents.OnTeamsSet -= GetTeams;
            Eventbus.CombatEvents.OnActorKilled -= ExchangeActors;
        }
    }
}