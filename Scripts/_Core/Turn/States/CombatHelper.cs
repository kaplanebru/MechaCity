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

        private CombatTimingData timingData;
        private bool pairsReversed = false;

        public void Register()
        {
            timingData = ScriptableObject.CreateInstance<CombatTimingData>(); //todo: bunu dışardan almalı
        }

        public void Subscribe(List<int> towers)
        {
            combatPairsCreator = new CombatPairsCreator(Data.CombatPairs);
            
            Eventbus.CombatEvents.OnTowerKilled += TowerDeath;
            BpEventbus.SubscriberEvents.OnReverseAction += ReversePairs;
            
            _towers = towers;
            _towers?.ForEach(at => AllTowers.GetTower(at).ToOriginalColor());
        }

        private bool _hasDeathTower = false;
        private void TowerDeath(TowerData obj)
        {
            _hasDeathTower = true;
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
                AllTowers.GetTower(pair.MainTowerData.UniqID).ToSelectionColor();
            else
                AllTowers.GetTower(pair.MainTowerData.UniqID).ToOriginalColor();
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
            yield return new WaitForSeconds(timingData.cameraDelay);
            Eventbus.CombatEvents.OnCombatStarted?.Invoke();


            for (int i = 0; i < AllTowers.TowersCount; i++)
            {
                var pair = Data.CombatPairs[i];
                SetSelectionColor(pair);

                yield return new WaitForSeconds(Data.selectionDelay);


                if (pair.Combat())
                {
                    yield return new WaitUntil(() => pair.CombatCompleted);
                    if (_hasDeathTower)
                    {
                       
                        _hasDeathTower = false;
                        yield return new WaitForSeconds(timingData.deathTime);
                        Debug.Log("Death tower");
                    }
                }
                else
                {
                    yield return new WaitForSeconds(timingData.skipDelay);
                }
                
                yield return new WaitForSeconds(Data.afterCombatDelay);

                Eventbus.CombatEvents.OnFire?.Invoke(Data.cursorDuration);
                yield return new WaitForSeconds(Data.cursorDuration);
                SetSelectionColor(pair, false);
            }

            Eventbus.CombatEvents.OnCombatEnding?.Invoke();
            yield return new WaitForSeconds(0.5f);
            AllTowers.RestoreBullets();

            EndCombat();
        }

        void EndCombat()
        {
            Eventbus.CombatEvents.OnCombatTerminated?.Invoke();
            Unsubscribe();
        }

        void DeselectAlteredTowers()
        {
            _towers?.ForEach(t => AllTowers.GetTower(t).ToOriginalColor());
        }

        public void Unsubscribe()
        {
            DeselectAlteredTowers();
            BpEventbus.SubscriberEvents.OnReverseAction -= ReversePairs;
            Eventbus.CombatEvents.OnTowerKilled -= TowerDeath;
            BpEventbus.ActionEvents.OnRestoreSelectionAmount?.Invoke();
        }
    }
}