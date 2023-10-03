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
        public List<Tower> AlteredTowers = new();
        public List<TowerGridRelationModel> DeadTowers = new();
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
            TransferData.AlteredTowers.ForEach(CreateCombatPairByTower);
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
            RestoreBullets();
            CompleteAction();
        }

        public void CreateCombatPairByTower(Tower tower)
        {
            OrderLinkedTowersByDistance(tower);

            for (var i = 0; i < tower.Data.LinkedTowers.Count; i++)
            {
                var linkedTower = tower.Data.LinkedTowers[i];
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
            tower.Data.LinkedTowers =
                tower.Data.LinkedTowers.OrderBy(other => Mathf.Abs(tower.Data.SlotId - other.Data.SlotId)).ToList();
        }
        
        void RestoreBullets()
        {
            foreach (var team in teams)
            {
                team.Value.Data.Towers.ForEach(t => t.RestoreBullets());
            }
        }

        void DeselectAlteredTowers() //TODO: At the end of animation
        {
            TransferData.AlteredTowers.ForEach(t => t.towerParts.SetColor(t.Data.TeamTowerData.DefaultMaterial));
        }

        private void SetDetachedPairsRestored()
        {
            Data.pairsRestored = true;
        }

        private void LatestDeadTower(Tower tower)
        {
            Data.latestDeadTower = tower;
            Data.CombatPairs.RemoveAll(p => p.Contains(Data.latestDeadTower));
        }

        public override void Unsubscribe()
        {
            DeselectAlteredTowers();
            Eventbus.CombatEvents.OnTowerKilled -= LatestDeadTower;
            Eventbus.CombatEvents.OnMatchesRestored -= SetDetachedPairsRestored;
        }
    }
}