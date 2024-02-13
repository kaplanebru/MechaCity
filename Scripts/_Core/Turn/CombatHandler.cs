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
        public List<CombatGroup> CombatPairs = new();
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
            AllTowers.Towers.ForEach(t=> CreateCombatPairByTower(AllTowers.GetData(t.Data.UniqID)));
            //TransferData.AlteredTowers.ForEach(t=> CreateCombatPairByTower(AllTowers.GetData(t)));
            StartCoroutine(nameof(FireRoutine));
        }

        IEnumerator FireRoutine()
        {
            
            print(Data.CombatPairs.Count);
            Data.CombatPairs = Data.CombatPairs.OrderBy(p => p.OtherTowerData.SlotId).ToList();
            
            Eventbus.CombatEvents.OnFire?.Invoke(Data.fireDelay);

            int j = 0;
            while (true)
            {
                if (j >= Data.CombatPairs.Count) break;
                var pair = Data.CombatPairs[j];

                pair.Combat(Data.projectileSpeed);
                yield return new WaitUntil(() => pair.CombatCompleted);
                yield return new WaitForSeconds(Data.fireDelay);

                if (pair.OtherTowerData == Data.latestDeadTower)
                {
                    yield return new WaitUntil(() => Data.pairsRestored);
                    Data.pairsRestored = false;
                    Data.latestDeadTower = null;

                    j = pair.OtherTowerData.SlotId;
                }
                else
                    j++;
            }

            yield return new WaitForSeconds(0.1f);
            AllTowers.RestoreBullets();
            CompleteAction();
        }
        

        public void CreateCombatPairByTower(TowerData tower)
        {
            
            OrderLinkedTowersByDistance(tower);

            for (var i = 0; i < tower.LinkedTowerIDs.Count; i++)
            {
                var linkedTower = AllTowers.GetData(tower.LinkedTowerIDs[i]);
                AddToPairs(tower, linkedTower);
            }
        }

        void AddToPairs(TowerData tower1, TowerData tower2)
        {
            Data.CombatPairs.Add(new CombatGroup(tower1, tower2));
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