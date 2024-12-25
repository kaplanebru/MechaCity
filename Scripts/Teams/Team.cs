using System;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using Towers;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Teams
{
    public class Team //: MonoBehaviour //<TPlayerData>: MonoBehaviour where TPlayerData : TeamData
    {
        public TeamData Data;

        public Team(TeamData data)
        {
            Data = data;
        }
        
        public void DistributeTeamActors()
        {
            Data.Actors.Clear();

            foreach (var actor in ActorDB.Registry.Values)
            {
                if (actor.TeamType == Data.TeamType)
                {
                    Data.Actors.Add(actor);
                }
            }
        }
        
        public void TakeActorFromRival(ActorData actor)
        {
            Data.Actors.Add(actor);

            foreach (var tower in actor.Towers)
            {
               tower.VisualData.SetTeamVisuals(Data.teamColorData);
            }

            foreach (var data in actor.TowerNumericDatas)
            {
                data.TeamType = actor.TeamType;
            }
        }

        public void RemoveTower(ActorData actor)
        {
            Data.Actors.Remove(actor);
        }
        
        // todo: separate

       
    }

    public static class TeamEvents
    {
        public static Action<Dictionary<TeamState, Team>> OnTeamsSent;
        public static Action OnTeamsRequest;
    }
}