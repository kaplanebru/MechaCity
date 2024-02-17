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

        [ReadOnly] public float afterCombatDelay = .3f;
        public float selectionDelay = 0.3f;
        public float cursorDuration = 0.5f;

        public void CreateCombatPairs()
        {
            CombatPairs.Clear();
            AllTowers.Towers.ForEach(t => CombatPairByTower(AllTowers.GetData(t.Data.UniqID)));
        }
        
        //TODO: CombatPairCreator.cs

        public void CreateReverseCombatPairs()
        {
            CombatPairs.Clear();
            AllTowers.ReverseLink(true);
            
            for (int i = AllTowers.TowersCount - 1; i >= 0; i--)
            {
                CombatPairByTower(AllTowers.GetData(i));
                Debug.Log(i);
            }
        }
        
        public void CombatPairByTower(TowerData tower)
        {
            OrderLinkedTowersByID(tower);

            foreach (var id in tower.LinkedTowerIDs)
            {
                var linkedTower = AllTowers.GetData(id);
                AddToPair(tower, linkedTower);
            }
        }
        
      

        void AddToPair(TowerData tower1, TowerData tower2)
        {
            CombatPairs.Add(new CombatPair(tower1, tower2));
        }

        void OrderLinkedTowersByID(TowerData tower)
        {
            tower.LinkedTowerIDs =
                tower.LinkedTowerIDs.OrderBy(other => Mathf.Abs(tower.SlotId - AllTowers.GetData(other).SlotId))
                    .ToList();
        }
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
        }

        public override void ProcessIncomingData(BaseTurnTransferData data)
        {
            var incomingData = (TowerGroupTransferData) data;
            TransferData.AlteredTowers = incomingData.TowerGroup;
        }

        public void ConstantSetup()
        {
           // Data.CreateCombatPairs();
            Data.CreateReverseCombatPairs();
        }

        public override void Setup()
        {
            TransferData.AlteredTowers.ForEach(at => AllTowers.GetTower(at).ResetColor());
            StartCoroutine(nameof(FireRoutine));
        }

        void Select(CombatPair pair, bool select = true)
        {
            if (select)
                AllTowers.GetTower(pair.MainTowerData.UniqID).SelectColor();
            else
                AllTowers.GetTower(pair.MainTowerData.UniqID).ResetColor();
        }

        IEnumerator FireRoutine()
        {
            Eventbus.CombatEvents.OnCombatReady?.Invoke();
            yield return new WaitForSeconds(timingData.cameraDelay);
            Eventbus.CombatEvents.OnCombatStarted?.Invoke();


            for (int i = 0; i < AllTowers.TowersCount; i++)
            {
                var pair = Data.CombatPairs[i];
                Select(pair);

                yield return new WaitForSeconds(Data.selectionDelay);


                pair.Combat(timingData);

                yield return new WaitUntil(() => pair.CombatCompleted);
                yield return new WaitForSeconds(Data.afterCombatDelay);

                Eventbus.CombatEvents.OnFire?.Invoke(Data.cursorDuration);
                yield return new WaitForSeconds(Data.cursorDuration);
                Select(pair, false);
            }

            Eventbus.CombatEvents.OnCombatEnding?.Invoke();
            yield return new WaitForSeconds(0.5f);
            AllTowers.RestoreBullets();
            Eventbus.CombatEvents.OnCombatTerminated?.Invoke();
            CompleteAction();
        }

        void DeselectAlteredTowers() //TODO: At the end of animation
        {
            TransferData.AlteredTowers.ForEach(t =>
                AllTowers.GetTower(t).towerParts.SetColor(AllTowers.GetTower(t).Data.TeamTowerData.DefaultMaterial));
        }

        public override void Unsubscribe()
        {
            DeselectAlteredTowers();
        }
    }
}