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
    public class TeamsController //: BaseTurnHelper
    {
       public static Team[] Teams; //turnmanagerdan da alınabilir
       public void Subscribe()
        {
            TeamEvents.OnTeamsSet += SetTeams;
            Eventbus.CombatEvents.OnActorKilled += ExchangeActors;
        }

        public void SetTeams(Team[] teams)
        {
            Teams = teams;
        }

        Team GetTeamDataByTeamType(TeamType type) => Teams.First(team => team.Data.TeamType == type);

        public static Team GetOtherTeam(TeamType type)
        {
            Team teamToSend = null;
            foreach (var team in Teams)
            {
                if(team.Data.TeamType == type)
                    continue;
                teamToSend = team;
            }

            return teamToSend;
        }

        private uint _deadActorID;

        private async void ExchangeActors(uint actorID)
        {
            _deadActorID = actorID;
            var actor = ActorDB.Registry[actorID];
            ExchangeActor(actor);

            await DelayMaker.WaitForSeconds(1);
            ExchangeCompletedCall();
            //Invoke(nameof(ExchangeCompletedCall), 1f); //todo: temporary
        }

        private void ExchangeActor(ActorData deadActor)
        {
            Team oldTeam = GetTeamDataByTeamType(deadActor.TeamType);
            Team newTeam = Teams.FirstOrDefault(t => t != oldTeam);

            oldTeam.RemoveTower(deadActor);
            newTeam.TakeActorFromRival(deadActor);
        }

        void ExchangeCompletedCall()
        {
            Eventbus.CombatEvents.OnTeamSwitched?.Invoke(_deadActorID);
        }

        public void Unsubscribe()
        {
            TeamEvents.OnTeamsSet -= SetTeams;
            Eventbus.CombatEvents.OnActorKilled -= ExchangeActors;
        }
    }
}