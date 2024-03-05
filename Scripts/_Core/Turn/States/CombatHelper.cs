using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Enums;
using Network;
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

    public class CombatHelper
    {
        private readonly CombatData Data = new();
        private CombatPairsCreator combatPairsCreator;
        private List<int> _towers;
        private TurnManager _turnManager;

        private bool pairsReversed = false;


        public void Subscribe(List<int> towers, TurnManager turnManager) //TODO: TM
        {
            combatPairsCreator = new CombatPairsCreator(Data.CombatPairs);
            BpEventbus.SubscriberEvents.OnReverseAction += ReversePairs;
            _towers = towers;
            _turnManager = turnManager;
            
            _towers?.ForEach(at => AllTowers.GetTower(at).ResetColor());
        }


        public void Fire()
        {
            _turnManager.StartCoroutine(this.FireRoutine());
        }

        public void SetCombatPairs()
        {
            combatPairsCreator.CreateCombatPairs(AllTowers.TowerDatas.ToList(), pairsReversed);
        }

        void ReversePairs()
        {
            pairsReversed = !pairsReversed;
            // Debug.Log("pairs reversed: " + pairsReversed);
            SetCombatPairs();
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
            yield return new WaitForSeconds(_turnManager.timingData.cameraDelay);
            Eventbus.CombatEvents.OnCombatStarted?.Invoke();


            for (int i = 0; i < AllTowers.TowersCount; i++)
            {
                var pair = Data.CombatPairs[i];
                Select(pair);

                yield return new WaitForSeconds(Data.selectionDelay);


                pair.Combat(_turnManager.timingData);

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
                AllTowers.GetTower(t).towerParts.SetColor(AllTowers.GetTower(t).Data.TeamTowerData.DefaultMaterial));
        }
        
        public void Unsubscribe()
        {
            DeselectAlteredTowers();
            BpEventbus.SubscriberEvents.OnReverseAction -= ReversePairs;
        }
    }
}