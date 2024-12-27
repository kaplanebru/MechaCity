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
        private TowerData[] totalTowers;
        private List<ActorData> actors = new();
        //private bool isFirstTime = true;
        
        List<int> randomHeights = new();
        private Dictionary<int, int> randomHeightByTowerID = new();
        
        private float waitTime = 1f; //1den küçük olursa coroutineler de patlıyor (light ve health için geçerli olan)
        //TODO: RİSE ROUTİNE'İ HIZLANDIR VE ONA BAĞLI TWEENLERI DE AYNI ŞKEİLDE AYARLA HATTA SET BY SPEED YAP
        private int frequence = 3;
        public void Execute(params object[] obj)
        {
            var rivalTeam = TeamEvents.OnSingleTeamDemand?.Invoke(TeamState.RivalTeam);
            SetTowers(rivalTeam.Data.Actors);
            CreateEarthquake();
            CommitEarthquakePhase();
        }

        private async void CreateEarthquake()
        {
            for (int i = 0; i < frequence; i++)
            {
                CommitEarthquakePhase();
                await DelayMaker.WaitForSeconds(waitTime);
                Debug.Log(i);
              
            }
        }

        void ResetCollections()
        {
            randomHeights.Clear();
            randomHeightByTowerID.Clear();
        }

        void SetTowers(List<ActorData> selectedActors)
        {
            actors = selectedActors;
            totalHeight = actors.Sum(a => a.GetTotalHeight());
            towerAmount = actors.Sum(a => a.TowerAmount);
            totalTowers = actors.SelectMany(a => a.Towers).ToArray();
        }

        void CommitEarthquakePhase() //rakibe atılsın sadece
        {
            ResetCollections();
            SetRandomHeight(totalHeight, towerAmount);
            //ExecuteHeights();
        
            //todo: varsa random lock da eklenir
        }


        void SetRandomHeight(int totalHeight, int towerAmount)
        {
            int newHeight;
            if (towerAmount == 1)
            {
                newHeight = totalHeight;
                randomHeights.Add(newHeight);
                
                if(!TryMatchTowersWithHeights())
                    return;
                ExecuteHeights();
                // if (isFirstTime)
                // {
                //     ExecuteHeights();
                //     isFirstTime = false;
                // }
                return;
            }

            int max = totalHeight - (towerAmount - 1);
            newHeight = Random.Range(1, max + 1); //todo: Oyunun max heightiyle de sınırlanır
            randomHeights.Add(newHeight);

            SetRandomHeight(totalHeight - newHeight, towerAmount - 1);
        }

      

        private bool TryMatchTowersWithHeights()
        {
            for (var i = 0; i < totalTowers.Length; i++)
            {
                var towerNumeric = totalTowers[i].NumericData;
                if(IsEqualInHeight(towerNumeric, randomHeights[i] )) 
                   return false;
                randomHeightByTowerID.Add(towerNumeric.UniqID, randomHeights[i]);
            }
            return true;
        }

        private bool IsEqualInHeight(TowerNumericData towerNumeric, int randomHeight)
        {
            if (towerNumeric.Height == randomHeight) //eşit gelmemesi için
            {
                ResetCollections();
                SetRandomHeight(totalHeight, towerAmount);
                return true;
            }
            return false;
        }


        void ExecuteHeights()
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