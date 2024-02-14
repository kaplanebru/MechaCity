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
        [ReadOnly] public bool pairsRestored = false;
        

        [ReadOnly] public float shootDuration = 1;
        [ReadOnly] public float afterCombatDelay = .3f;
        public float skipDelay = 0.3f;
        public float selectionDelay = 0.3f;
        public float cursorDuration = 0.5f;

    }

    public class CombatHandler : BaseTurnHandler, ITurnActionHandler<CombatTransferData>
    {
        public CombatTransferData TransferData { get; private set; }
        public CombatTimingData timingData;

        public override TurnHandlerType HandlerType => TurnHandlerType.Combat;
        private readonly CombatData Data = new();


        public override void OnHandlerEnabled()
        {
            TransferData = new();

            //Eventbus.CombatEvents.OnTowerKilled += LatestDeadTower;
            //Eventbus.CombatEvents.OnMatchesRestored += SetDetachedPairsRestored;
        }

        public override void ProcessIncomingData(BaseTurnTransferData data)
        {
            var incomingData = (TowerGroupTransferData) data;
            TransferData.AlteredTowers = incomingData.TowerGroup;
        }

        public override void Setup()
        {
            //RemoveAlteredCombatPairs();
            Data.CombatPairs.Clear();
            AllTowers.Towers.ForEach(t => CreateCombatPairByTower(AllTowers.GetData(t.Data.UniqID)));
            //TransferData.AlteredTowers.ForEach(t=> CreateCombatPairByTower(AllTowers.GetData(t)));
            StartCoroutine(nameof(FireRoutine));
        }

        IEnumerator FireRoutine()
        {
            //print(Data.CombatPairs.Count);
            //Data.CombatPairs = Data.CombatPairs.OrderBy(p => p.OtherTowerData.UniqID).ToList(); //SlotId idi

            for (int i = 0; i < AllTowers.TowersCount; i++)
            {
                yield return new WaitForSeconds(Data.selectionDelay);
                var pair = Data.CombatPairs[i];
               
                pair.Combat(timingData);

                yield return new WaitUntil(() => pair.CombatCompleted);
                yield return new WaitForSeconds(Data.afterCombatDelay);

                Eventbus.CombatEvents.OnFire?.Invoke(Data.cursorDuration);
                yield return new WaitForSeconds(Data.cursorDuration);
            }
            
            //
            //     if (pair.OtherTowerData == Data.latestDeadTower)
            //     {
            //         yield return new WaitUntil(() => Data.pairsRestored);
            //         Data.pairsRestored = false;
            //         Data.latestDeadTower = null;
            //
            //         j = pair.OtherTowerData.SlotId;
         

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
            Data.CombatPairs.Add(new CombatPair(tower1, tower2));
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
                tower.LinkedTowerIDs.OrderBy(other => Mathf.Abs(tower.SlotId - AllTowers.GetData(other).SlotId))
                    .ToList();
        }

        void DeselectAlteredTowers() //TODO: At the end of animation
        {
            TransferData.AlteredTowers.ForEach(t =>
                AllTowers.GetTower(t).towerParts.SetColor(AllTowers.GetTower(t).Data.TeamTowerData.DefaultMaterial));
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