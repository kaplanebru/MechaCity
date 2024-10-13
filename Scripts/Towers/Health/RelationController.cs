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
        
        public static void SetLinkedTowers(List<uint> tempTowerIDs) //ters de gelebilir
        {
            ResetAllLinks();
            for (var i = 0; i < AllTowers.TowersCount; i++)
            {
                var mainID = tempTowerIDs[i];
                var nextIDInOrder = tempTowerIDs[(i + 1) % AllTowers.TowersCount];
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