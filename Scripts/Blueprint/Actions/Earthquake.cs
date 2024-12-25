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
        private int totalHeight;
        private int towerAmount;
        public void Execute(params object[] obj)
        {
            var rivalTeam = TeamEvents.OnSingleTeamDemand?.Invoke(TeamState.RivalTeam);
            StartEarthquake(rivalTeam.Data.Actors);
        }

        void StartEarthquake(List<ActorData> actors) //rakibe atılsın sadece
        {
            totalHeight = actors.Sum(a => a.GetTotalHeight());
            towerAmount = actors.Sum(a => a.TowerAmount);

            randomHeights.Clear();
            SetRandomHeight(totalHeight, towerAmount);
            MatchTowersWithHeights(actors);
            //todo: varsa random lock da eklenir
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

            SetRandomHeight(totalHeight - newHeight, towerAmount - 1);
        }

        private Dictionary<int, int> randomHeightByTowerID = new();
        private TowerData[] totalTowers;

        void MatchTowersWithHeights(List<ActorData> actors)
        {
            randomHeightByTowerID.Clear();
            totalTowers = actors.SelectMany(a => a.Towers).ToArray();
            for (var i = 0; i < totalTowers.Length; i++)
            {
                var towerNumeric = totalTowers[i].NumericData;
                if(IsEqualInHeight(towerNumeric, randomHeights[i] ))break;
                    // if (towerNumeric.Height == randomHeights[i]) //eşit gelmemesi için
                // {
                //     randomHeightByTowerID.Clear();
                //     randomHeights.Clear();
                //     SetRandomHeight(totalHeight, towerAmount);
                //     break;
                // }
                randomHeightByTowerID.Add(towerNumeric.UniqID, randomHeights[i]);
            }
        }

        private bool IsEqualInHeight(TowerNumericData towerNumeric, int randomHeight)
        {
            if (towerNumeric.Height == randomHeight) //eşit gelmemesi için
            {
                randomHeightByTowerID.Clear();
                randomHeights.Clear();
                SetRandomHeight(totalHeight, towerAmount);
                return true;
            }

            return false;
        }


        void ExecuteHeights(List<ActorData> actors)
        {
            foreach (var actor in actors)
            {
                if (actor.Type == ActorType.MultiTower)
                {
                    foreach (var towerData in actor.Towers)
                    {
                        SetNewHeight(towerData);
                    }
                    DoubleTowerEqualizer.Equalize(actor.Towers);
                }
                else
                {
                    var towerObject = SetNewHeight(actor.Towers[0]);
                    towerObject.StartRiseFallRoutine(true);
                }
            }
        }

        private TowerObject SetNewHeight(TowerData tower)
        {
            var towerID = tower.NumericData.UniqID;
            var towerObject = AllTowers.GetTower(towerID);

            towerObject.Data.SetHeightAutonomously(randomHeightByTowerID[towerID]);
            return towerObject;
        }
    }
}