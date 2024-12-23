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

        void Earthquake() //rakibe atılsın sadece
        {
            var totalHeight = Data.Actors.Sum(a => a.GetTotalHeight());
            var towerAmount = Data.Actors.Sum(a => a.TowerAmount);

            randomHeights.Clear();
            SetRandomHeight(totalHeight, towerAmount);
            //Equalize
            //Distribute heights to towers
        }
        
        List<int> randomHeights = new();

        void SetRandomHeight(int totalHeight, int towerAmount)
        {
            int newHeight;
            if (towerAmount == 1)
            {
                newHeight = totalHeight;
                randomHeights.Add(newHeight);
                return;
            }
            
            int max = totalHeight - (towerAmount - 1);
            newHeight = Random.Range(1, max + 1);
            randomHeights.Add(newHeight);
            
            SetRandomHeight(totalHeight-newHeight, towerAmount-1);
        }
    }

    public static class TeamEvents
    {
        public static Action<Dictionary<TeamState, Team>> OnTeamsSent;
        public static Action OnTeamsRequest;
    }
}