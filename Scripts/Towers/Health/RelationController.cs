using System.Collections.Generic;
using Towers;

namespace Actor
{
    public class RelationController : ActorController
    {
        public RelationController(ActorHolder holder) : base(holder) {}
        
        public override void Subscribe()
        {
            Eventbus.LinkEvents.OnCreatingCombatPairs += SetLinkedTowers;
        }
        
        public static void SetLinkedTowers(List<uint> actors) //ters de gelebilir
        {
            
            ResetAllLinks();
            var actorsAmount = actors.Count;
            for (var i = 0; i < actorsAmount ; i++)
            {
                var mainID = actors[i];
                var nextIDInOrder = actors[(i + 1) % actorsAmount];
                //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da
                
                ActorHolder.Registry[mainID].SetLinkedTowers(nextIDInOrder); //burda patlar, double'ın elemanı olup registeryde bulunmayabilir!
            }
        }

        private static void ResetAllLinks()
        {
            foreach (var registry in ActorHolder.Registry.Values)
            {
                registry.LinkedActors.Clear();
            }
        }

        public override void Unsubscribe()
        {
            Eventbus.LinkEvents.OnCreatingCombatPairs -= SetLinkedTowers;
        }

    }
}