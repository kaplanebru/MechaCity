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
        public Tower latestDeadTower;
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
            TransferData.AlteredTowers.ForEach(t=> CreateCombatPairByTower(AllTowers.GetTower(t)));
            //RestoreBullets();
            StartCoroutine(nameof(FireRoutine));
        }

        IEnumerator FireRoutine()
        {
            Data.CombatPairs = Data.CombatPairs.OrderBy(p => p.Perpetrator.Data.SlotId).ToList();

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

                    j = pair.Perpetrator.Data.SlotId;
                }
                else
                    j++;
            }

            yield return new WaitForSeconds(0.1f);
            AllTowers.RestoreBullets();
            //RestoreBullets();
            CompleteAction();
        }

        public void CreateCombatPairByTower(Tower tower)
        {
            //OrderLinkedTowersByDistance(tower);

            for (var i = 0; i < tower.Data.LinkedTowerIDs.Count; i++)
            {
                //var linkedTower = tower.Data.LinkedTowerIDs[i];
                var linkedTower = AllTowers.GetTower(tower.Data.LinkedTowerIDs[i]);
                if (tower.Data.Height > linkedTower.Data.Height)
                {
                    if (!tower.Data.CanShoot) continue;
                    AddToPairs(tower, linkedTower);
                }
                else if (linkedTower.Data.Height > tower.Data.Height)
                {
                    if (!linkedTower.Data.CanShoot) continue;
                    AddToPairs(linkedTower, tower);
                }
                else
                    AddToPairs(linkedTower, tower, true);
            }
        }

        void AddToPairs(Tower tower1, Tower tower2, bool isEven = false)
        {
            Data.CombatPairs.Add(new CombatPair(tower1, tower2, isEven));
            if (!isEven)
                tower1.Data.BulletAmount--;
        }

        void RemoveAlteredCombatPairs()
        {
            foreach (var alteredTower in TransferData.AlteredTowers)
            {
                Data.CombatPairs.RemoveAll(pair => pair.Contains(alteredTower));
            }
        }

        void OrderLinkedTowersByDistance(Tower tower)
        {
            tower.Data.LinkedTowerIDs =
                tower.Data.LinkedTowerIDs.OrderBy(other => Mathf.Abs(tower.Data.SlotId - AllTowers.GetTower(other).Data.SlotId)).ToList();
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

        private void LatestDeadTower(Tower tower)
        {
            Data.latestDeadTower = tower;
            Data.CombatPairs.RemoveAll(p => p.Contains(Data.latestDeadTower.Data.UniqID));
        }

        public override void Unsubscribe()
        {
            DeselectAlteredTowers();
            Eventbus.CombatEvents.OnTowerKilled -= LatestDeadTower;
            Eventbus.CombatEvents.OnMatchesRestored -= SetDetachedPairsRestored;
        }
    }
}