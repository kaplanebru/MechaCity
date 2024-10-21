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


        public void Subscribe()
        {
            combatPairsCreator = new CombatPairsCreator(CombatPairs);
            Eventbus.ActorEvents.OnRelationsSet += SetCombatPairs;
        }

        public CombatPair GetPairByIndex(int index) => CombatPairs[index];

        public int PairAmount => CombatPairs.Count;

        public void ResetCombatCompletedForAll()
        {
            CombatPairs.ForEach(p=> p.CombatCompleted = false);
        }

        private void SetCombatPairs(List<uint> actors, bool isReversed)
        {
            combatPairsCreator.CreateCombatPairs(actors, isReversed);
            Eventbus.CombatEvents.OnPairsSet?.Invoke();
        }
        
        public void Unsubscribe()
        {
            Eventbus.ActorEvents.OnRelationsSet -= SetCombatPairs;
        }

    }

}
