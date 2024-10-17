using System.Collections.Generic;
using System.Linq;
using Actor;
using DataModels;
using Towers;

namespace Turn
{
    public class CombatPairController
    {
        private List<CombatPair> CombatPairs = new();
        private CombatPairsCreator combatPairsCreator;
        private bool pairsReversed = false;

        
        public void Subscribe()
        {
            combatPairsCreator = new CombatPairsCreator(CombatPairs);
            BpEventbus.SubscriberEvents.OnReverseAction += ReversePairs;
            Eventbus.ActorEvents.OnRegistryUpdate += SetCombatPairs; 

        }

        public CombatPair GetPairByIndex(int index) => CombatPairs[index];

        public int PairAmount => CombatPairs.Count;

        public void ResetCombatCompletedForAll()
        {
            CombatPairs.ForEach(p=> p.CombatCompleted = false);
        }

        public void SetCombatPairs()
        {
            combatPairsCreator.CreateCombatPairs(ActorHolder.Registry, pairsReversed);
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
            Eventbus.ActorEvents.OnRegistryUpdate -= SetCombatPairs;
        }

    }

}
