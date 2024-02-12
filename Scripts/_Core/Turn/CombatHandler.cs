using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Enums;
using Towers;
using Unity.Collections;
using UnityEngine;

namespace Turn
{
    public class CombatTransferData : BaseTurnTransferData // = sıfırlanacak data
    {
        public List<int> AlteredTowers = new();
    }


    [Serializable]
    public class CombatData
    {
        public List<CombatPair> CombatPairs = new();
        public TowerData latestDeadTower;
        [ReadOnly] public float projectileSpeed = 1;
        [ReadOnly] public bool pairsRestored = false;
        [ReadOnly] public float fireDelay = 0.5f;
    }

    public class CombatHandler : BaseTurnHandler, ITurnActionHandler<CombatTransferData>
    {
        public CombatTransferData TransferData { get; private set; }
        public override TurnHandlerType HandlerType => TurnHandlerType.Combat;
        private readonly CombatData Data = new();


        public override void OnHandlerEnabled()
        {
            TransferData = new();

            Eventbus.CombatEvents.OnTowerKilled += LatestDeadTower;
            Eventbus.CombatEvents.OnMatchesRestored += SetDetachedPairsRestored;
        }
        
        public override void ProcessIncomingData(BaseTurnTransferData data)
        {
            var incomingData = (TowerGroupTransferData) data;
            TransferData.AlteredTowers = incomingData.TowerGroup;
        }

        public override void Setup()
        {
            RemoveAlteredCombatPairs();
            TransferData.AlteredTowers.ForEach(t=> CreateCombatPairByTower(AllTowers.GetData(t)));
            //RestoreBullets();
            StartCoroutine(nameof(FireRoutine));
        }

        IEnumerator FireRoutine()
        {
            Data.CombatPairs = Data.CombatPairs.OrderBy(p => p.Perpetrator.SlotId).ToList();

            int j = 0;
            while (true)
            {
                if (j >= Data.CombatPairs.Count) break;
                var pair = Data.CombatPairs[j];

                pair.Combat(Data.projectileSpeed);
                yield return new WaitUntil(() => pair.CombatCompleted);
                yield return new WaitForSeconds(Data.fireDelay);

                if (pair.Victim == Data.latestDeadTower)
                {
                    yield return new WaitUntil(() => Data.pairsRestored);
                    Data.pairsRestored = false;
                    Data.latestDeadTower = null;

                    j = pair.Perpetrator.SlotId;
                }
                else
                    j++;
            }

            yield return new WaitForSeconds(0.1f);
            AllTowers.RestoreBullets();
            //RestoreBullets();
            CompleteAction();
        }

        void CreateCombatPairsById() //is even ve initialda sadece
        {
            for (var i = 0; i < AllTowers.Datas.Count-1; i++)
            {
                Data.CombatPairs.Add(new CombatPair(AllTowers.Datas[i], AllTowers.Datas[i+1], true));
            }
            Data.CombatPairs.Add(new CombatPair(AllTowers.Datas.Last(), AllTowers.Datas.First(), true));
        }

        public void CreateCombatPairByTower(TowerData tower)
        {
            //for (int i = 0; i < Grids[tower.TeamTowerData.TeamType].Slots[tower.SlotId].LinkedSlotIDs.Count; i++)
            
            OrderLinkedTowersByDistance(tower);
            

            for (var i = 0; i < tower.LinkedTowerIDs.Count; i++)
            {
                //var linkedTower = tower.Data.LinkedTowerIDs[i];
                var linkedTower = AllTowers.GetData(tower.LinkedTowerIDs[i]);
                if (tower.Height > linkedTower.Height)
                {
                    if (!tower.CanShoot) continue;
                    AddToPairs(tower, linkedTower);
                }
                else if (linkedTower.Height > tower.Height)
                {
                    if (!linkedTower.CanShoot) continue;
                    AddToPairs(linkedTower, tower);
                }
                else
                    AddToPairs(linkedTower, tower, true);
            }
        }

        void AddToPairs(TowerData tower1, TowerData tower2, bool isEven = false)
        {
            Data.CombatPairs.Add(new CombatPair(tower1, tower2, isEven));
            if (!isEven)
                tower1.BulletAmount--;
        }

        void RemoveAlteredCombatPairs()
        {
            foreach (var alteredTower in TransferData.AlteredTowers)
            {
                Data.CombatPairs.RemoveAll(pair => pair.Contains(alteredTower));
            }
        }

        void OrderLinkedTowersByDistance(TowerData tower)
        {
            tower.LinkedTowerIDs =
                tower.LinkedTowerIDs.OrderBy(other => Mathf.Abs(tower.SlotId - AllTowers.GetData(other).SlotId)).ToList();
        }
        
        // void RestoreBullets()
        // {
        //     foreach (var team in teams)
        //     {
        //         team.Value.Data.TowerIds.ForEach(t => t.RestoreBullets());
        //     }
        // }

        void DeselectAlteredTowers() //TODO: At the end of animation
        {
            TransferData.AlteredTowers.ForEach(t => AllTowers.GetTower(t).towerParts.SetColor( AllTowers.GetTower(t).Data.TeamTowerData.DefaultMaterial));
        }

        private void SetDetachedPairsRestored()
        {
            Data.pairsRestored = true;
        }

        private void LatestDeadTower(TowerData tower)
        {
            Data.latestDeadTower = tower;
            Data.CombatPairs.RemoveAll(p => p.Contains(Data.latestDeadTower.UniqID));
        }

        public override void Unsubscribe()
        {
            DeselectAlteredTowers();
            Eventbus.CombatEvents.OnTowerKilled -= LatestDeadTower;
            Eventbus.CombatEvents.OnMatchesRestored -= SetDetachedPairsRestored;
        }
    }
}