using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Blueprint;
using Enums;
using Teams;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class Earthquake : IBpAction
    {
        public void Execute(params object[] obj)
        {
            var rivalTeam = TeamEvents.OnSingleTeamDemand?.Invoke(TeamState.RivalTeam);
            StartEarthquake(rivalTeam.Data.Actors);
        }
        
        void StartEarthquake(List<ActorData> actors) //rakibe atılsın sadece
        {
            var totalHeight = actors.Sum(a => a.GetTotalHeight());
            var towerAmount = actors.Sum(a => a.TowerAmount);

            randomHeights.Clear();
            SetRandomHeight(totalHeight, towerAmount);
            MatchTowersWithHeights(actors); 
            //todo: varsa random lock da eklenir
            //ExecuteNewHeights();
            ExecuteHeights(actors);
         
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
            newHeight = Random.Range(1, max + 1); //todo: Oyunun max heightiyle de sınırlanır
            randomHeights.Add(newHeight);
            
            SetRandomHeight(totalHeight-newHeight, towerAmount-1);
        }

        private Dictionary<int, int> randomHeightByTowerID = new();
        private TowerData[] totalTowers;
        void MatchTowersWithHeights(List<ActorData> actors)
        {
            randomHeightByTowerID.Clear();
            totalTowers = actors.SelectMany(a => a.Towers).ToArray();
            for (var i = 0; i < totalTowers.Length; i++)
            {
                randomHeightByTowerID.Add(totalTowers[i].NumericData.UniqID, randomHeights[i]);
            }
        }

        void ExecuteNewHeights()
        {
            foreach (var tower in totalTowers)
            {
                var towerID = tower.NumericData.UniqID;
                var towerObject = AllTowers.GetTower(towerID);
                
                towerObject.Data.SetHeightAutonomously(randomHeightByTowerID[towerID]);
                towerObject.StartRiseFallRoutine(true);
                //tower.UpdateHeight(); //set Height de olmalı bir yerlerde yok mu, en başta?
                //randomHeightByTowerID.Add(totalTowers[i].NumericData.UniqID, randomHeights[i]);
            }
        }

        void ExecuteHeights(List<ActorData> actors)
        {
            foreach (var actor in actors)
            {
                if (actor.Type == ActorType.MultiTower)
                {
                    foreach (var tower in actor.Towers)
                    {
                        var towerID = tower.NumericData.UniqID;
                        var towerObject = AllTowers.GetTower(towerID);
                        
                        towerObject.Data.SetHeightAutonomously(randomHeightByTowerID[towerID]);
                    }
                    DoubleTowerEqualizer.Equalize(actor.Towers);
                }
                else
                {
                    foreach (var tower in actor.Towers)
                    {
                        var towerID = tower.NumericData.UniqID;
                        var towerObject = AllTowers.GetTower(towerID);
                
                        towerObject.Data.SetHeightAutonomously(randomHeightByTowerID[towerID]);
                        towerObject.StartRiseFallRoutine(true);
                    }
                }
              
            }
        }

        void EqualizeDoubles(List<ActorData> actors)
        {
            foreach (var actor in actors)
            {
                if (actor.Type != ActorType.MultiTower) continue;

                foreach (var tower in actor.TowerNumericDatas)
                {
                    //3 tane olduğunu düşün nasıl equalize edicen?
                }
               
            }
        }
    }

}
