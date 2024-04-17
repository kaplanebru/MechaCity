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

  

    public class CombatHelper: IEnumeratorContainer
    {
        private readonly CombatData Data = new();
        private CombatPairsCreator combatPairsCreator;
        private List<int> _towers;


        private CombatTimingData timingData;
        private bool pairsReversed = false;


        public void Register()
        {
            timingData = ScriptableObject.CreateInstance<CombatTimingData>();
        }
        public void Subscribe(List<int> towers)
        {
            combatPairsCreator = new CombatPairsCreator(Data.CombatPairs);
            BpEventbus.SubscriberEvents.OnReverseAction += ReversePairs;
            _towers = towers;
            
            _towers?.ForEach(at => AllTowers.GetTower(at).ToOriginalColor());
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


        void Select(CombatPair pair, bool select = true)
        {
            if (select)
                AllTowers.GetTower(pair.MainTowerData.UniqID).ToSelectionColor();
            else
                AllTowers.GetTower(pair.MainTowerData.UniqID).ToOriginalColor();
        }

        public IEnumerator EnumeratorInstance()
        {
            if (MultiplayerSetter.IsTestingWithoutCombat) //TODO: later
            {
                yield return new WaitForSeconds(.5f);
                Eventbus.CombatEvents.OnCombatTerminated?.Invoke();
                Unsubscribe();
                yield break;
            }
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
            
            Unsubscribe();
        }

        void DeselectAlteredTowers()
        {
            _towers?.ForEach(t =>
                AllTowers.GetTower(t).ToOriginalColor());
            //towerParts.SetColor(AllTowers.GetTower(t).Data.TeamTowerData.DefaultMaterial));
        }
        
        public void Unsubscribe()
        {
            DeselectAlteredTowers();
            BpEventbus.SubscriberEvents.OnReverseAction -= ReversePairs;
        }

       
    }
}