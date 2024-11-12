using System.Collections.Generic;
using System.Linq;
using Actor;
using DataModels;
using Towers;

namespace Turn
{
    public class CombatPairController
    {
        private Dictionary<uint, List<CombatPair>> CombatPairs = new();
        private CombatPairsCreator combatPairsCreator;


        public void Subscribe()
        {
            combatPairsCreator = new CombatPairsCreator(CombatPairs);
            Eventbus.ActorEvents.OnRelationsSet += SetCombatPairs;
        }

        public List<CombatPair> GetPairByActorID(uint actorID) => CombatPairs[actorID];

        public int PairAmount => CombatPairs.Count;

        public void ResetCombatCompletedForAll()
        {
            foreach (var pairs in CombatPairs.Values)
            {
                pairs.ForEach(p=> p.CombatCompleted = false);
            }
        }

        private void SetCombatPairs(List<uint> actors, bool isReversed)
        {
            combatPairsCreator.CreateCombatPairs(actors, isReversed);
            Eventbus.CombatEvents.OnPairsSet?.Invoke(isReversed);
        }
        
        public void Unsubscribe()
        {
            Eventbus.ActorEvents.OnRelationsSet -= SetCombatPairs;
        }

    }

}
