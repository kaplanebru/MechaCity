using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Towers;
using UnityEngine;

namespace Turn
{
    public class CombatPairController
    {
        public List<CombatPair> CombatPairs = new();
        private CombatPairsCreator combatPairsCreator;
        private bool pairsReversed = false;

        
        public void Subscribe()
        {
            combatPairsCreator = new CombatPairsCreator(CombatPairs);
            BpEventbus.SubscriberEvents.OnReverseAction += ReversePairs;
        }

        public CombatPair GetPairByIndex(int index) => CombatPairs[index];
       
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
        
        public void Unsubscribe()
        {
            BpEventbus.SubscriberEvents.OnReverseAction -= ReversePairs;
        }

    }

}
