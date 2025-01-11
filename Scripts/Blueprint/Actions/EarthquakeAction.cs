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
    public class EarthquakeAction : IBpAction
    {
        private int totalHeight;
        private int towerAmount;

        private int startTotalHeight;
        private int startTowerAmount;
        
        private TowerNumericData[] towerNumericDatas;
        private List<TowerObject> towerObjects = new();
        private List<ActorData> actors = new();
       
        
        List<int> randomHeights = new();
        private Dictionary<int, int> randomHeightByTowerID = new();
        
        private float waitTime = .4f; //.5f
        private float towerTime = 1f;
        //TODO: RİSE ROUTİNE'İ HIZLANDIR VE ONA BAĞLI TWEENLERI DE AYNI ŞKEİLDE AYARLA HATTA SET BY SPEED YAP
        private int frequence = 4;
        private int stepTracker = 0;
        public void Execute(params object[] obj)
        {
            var rivalTeam = TeamEvents.OnSingleTeamDemand?.Invoke(TeamState.RivalTeam);
            SetTowers(rivalTeam.Data.Actors);
            PrepareAndExecuteEarthquake();
        }

        void SetStartSettings()
        {
            startTotalHeight = totalHeight;
            startTowerAmount = towerAmount;
        }

        bool HasTowersToGetReady()
        {
            if (actors.All(a => a.Type == ActorType.Standard))
                return false;
                
            foreach (var actor in actors)
            {
                if(actor.Type != ActorType.MultiTower) continue;
                foreach (var tower in actor.Towers)
                {
                    Eventbus.TowerEvents.OnBridgeDestroyRequest?.Invoke(tower.NumericData.UniqID);
                }
            }
            return true;
        }

        private async void PrepareAndExecuteEarthquake()
        {
            Eventbus.LinkEvents.OnLinkLoading?.Invoke(towerNumericDatas.Select(d=>d.UniqID).ToList());
            await DelayMaker.WaitForSeconds(1);
            
            if(HasTowersToGetReady())
                await DelayMaker.WaitForSeconds(1);
            
            MediatorEventbus.ChainMotionEvents.OnMotion?.Invoke();
            
            stepTracker = 0;
            for (int i = 0; i < frequence; i++)
            {
                stepTracker++;
                CommitEarthquakePhase();
                var delay = stepTracker == frequence ? towerTime : waitTime;
                await DelayMaker.WaitForSeconds(delay);
            }
            
            towerObjects.ForEach(t=>t.StopRiseFallRoutine());
            MediatorEventbus.ChainMotionEvents.OnStop?.Invoke();
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
            towerNumericDatas = actors.SelectMany(a => a.TowerNumericDatas).ToArray();
            foreach (var actor in actors)
            {
                foreach (var tower in actor.TowerNumericDatas)
                {
                    towerObjects.Add(AllTowers.GetTower(tower.UniqID));
                }
            }
            SetStartSettings();
        }

        void CommitEarthquakePhase() //rakibe atılsın sadece
        {
            ResetCollections();
            SetRandomHeight(totalHeight, towerAmount);

            //todo: varsa random lock da eklenir
        }


        void SetRandomHeight(int totalHeight, int towerAmount)
        {
            int newHeight;
            if (towerAmount == 1)
            {
                newHeight = totalHeight;
                if (newHeight > AllTowers.MaxTowerHeight)
                {
                    ResetCollections();
                    SetRandomHeight(startTotalHeight, startTowerAmount);
                    return;
                }
                randomHeights.Add(newHeight);

                if (TryMatchTowersWithHeights())
                {
                    SetTowersHeightData();
                    if (stepTracker == 1)
                    {
                        StartMotion();
                    }
                }
                
               
                return;
            }

            int max = totalHeight - (towerAmount - 1);
            max = Mathf.Min(max, AllTowers.MaxTowerHeight-1);
            newHeight = Random.Range(1, max + 1); //todo: Oyunun max heightiyle de sınırlanır
            randomHeights.Add(newHeight);

            SetRandomHeight(totalHeight - newHeight, towerAmount - 1);
        }

        void SetTowersHeightData()
        {
            foreach (var actor in actors)
            { 
                if(actor.Type == ActorType.Standard)
                    SetNewHeight(actor.Towers[0]);
                else
                {
                    if (stepTracker == frequence)
                    {
                        var equalizedHeights = DoubleTowerEqualizer.EqualizeHeights(actor.Towers.Select(t =>  randomHeightByTowerID[t.NumericData.UniqID]).ToArray());
                        for (var i = 0; i < actor.Towers.Length; i++)
                        {
                            var tower = actor.Towers[i];
                            tower.SetHeightAutonomously(equalizedHeights[i]);
                        }
                        Eventbus.TowerEvents.OnBridgeAttempt?.Invoke(actor.TowerIDs);
                    }
                    else
                    {
                        foreach (var tower in actor.Towers)
                        {
                            SetNewHeight(tower);
                        }
                    }
                }
            }
        }

        private bool TryMatchTowersWithHeights()
        {
            for (var i = 0; i < towerNumericDatas.Length; i++)
            {
                var towerNumeric = towerNumericDatas[i];
                if(IsEqualInHeight(towerNumeric, randomHeights[i])) 
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
                SetRandomHeight(startTotalHeight, startTowerAmount);
                return true;
            }
            return false;
        }

        void StartMotion()
        {
            foreach (var towerObject in towerObjects)
            {
                towerObject.StartRiseFallRoutine();
            }
        }

        private void SetNewHeight(TowerData tower)
        {
            var towerID = tower.NumericData.UniqID;
            tower.SetHeightAutonomously(randomHeightByTowerID[towerID]);
        }
        
      
    }
}