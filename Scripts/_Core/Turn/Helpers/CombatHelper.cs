using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Enums;
using Network;
using Testing;
using Towers;
using Unity.Collections;
using UnityEngine;

namespace Turn
{
    public class CombatData
    {
        public List<CombatPair> CombatPairs = new();

        [ReadOnly] public float afterCombatDelay = .3f;
        public float selectionDelay = 0.3f;
        public float cursorDuration = 0.5f;
    }

    public class CombatHelper : IEnumeratorContainer
    {
        private readonly CombatData Data = new();
        private CombatPairsCreator combatPairsCreator;
        private List<int> _towers;

        private CombatTimingData _timingData;
        private bool pairsReversed = false;
        

        public void GetTimingData(CombatTimingData combatTimingData)
        {
            _timingData = combatTimingData;
        }

        public void Subscribe(List<int> towers)
        {
            combatPairsCreator = new CombatPairsCreator(Data.CombatPairs);
            
            BpEventbus.SubscriberEvents.OnReverseAction += ReversePairs;
            
            _towers = towers;
            _towers?.ForEach(at => AllTowers.GetTower(at).ColorHandler.ToOriginalColor());
        }

        public void Fire()
        {
            Eventbus.CombatEvents.OnCoroutineTrigger?.Invoke(this);
        }

        public void SetCombatPairs()
        {
            combatPairsCreator.CreateCombatPairs(AllTowers.TowerDatas.ToList(), pairsReversed);
            Eventbus.CombatEvents.OnPairsSet?.Invoke();
        }

        void ReversePairs() //todo: bug, buraya uğramıyor
        {
            pairsReversed = !pairsReversed;
            SetCombatPairs();
        }


        void SetSelectionColor(CombatPair pair, bool select = true)
        {
            if (select)
                AllTowers.GetTower(pair.MainTowerData.UniqID).ColorHandler.ToSelectionColor();
            else
                AllTowers.GetTower(pair.MainTowerData.UniqID).ColorHandler.ToOriginalColor();
        }

        public IEnumerator FightRoutine()
        {
            if (MultiplayerSetter.IsTestingWithoutCombat)
            {
                yield return new WaitForSeconds(.5f);
                EndCombat();
                yield break;
            }

            Eventbus.CombatEvents.OnCombatReady?.Invoke();
            Eventbus.CombatEvents.OnCombatStarted?.Invoke();
            yield return new WaitForSeconds(_timingData.cameraDelay);

            for (int i = 0; i < AllTowers.TowersCount; i++)
            {
                Eventbus.CombatEvents.OnNextTower?.Invoke(Data.cursorDuration);
                yield return new WaitForSeconds(Data.cursorDuration);
                
                var pair = Data.CombatPairs[i];
                SetSelectionColor(pair);

                yield return new WaitForSeconds(Data.selectionDelay);
                
                if (pair.Combat())
                {
                    yield return new WaitUntil(() => pair.CombatCompleted);
                }
                else
                {
                    yield return new WaitForSeconds(_timingData.skipDelay);
                    yield return new WaitForSeconds(Data.afterCombatDelay);
                    SetSelectionColor(pair, false);
                }
            }

            Eventbus.CombatEvents.OnCombatEnding?.Invoke();
            yield return new WaitForSeconds(0.5f);
            AllTowers.RestoreBullets();

            EndCombat();
        }

        void EndCombat()
        {
            Data.CombatPairs.ForEach(p=> p.CombatCompleted = false);
            
            Eventbus.CombatEvents.OnCombatTerminated?.Invoke();
            Unsubscribe();
        }

        void DeselectAlteredTowers()
        {
            _towers?.ForEach(t => AllTowers.GetTower(t).ColorHandler.ToOriginalColor());
        }

        public void Unsubscribe()
        {
            DeselectAlteredTowers();
            BpEventbus.SubscriberEvents.OnReverseAction -= ReversePairs;
            BpEventbus.ActionEvents.OnRestoreSelectionAmount?.Invoke();
        }
    }
}