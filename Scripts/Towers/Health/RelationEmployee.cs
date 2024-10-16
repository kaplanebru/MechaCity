using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Actor
{
    public class RelationEmployee : ActorEmployee
    {
        public RelationEmployee(ActorHolder holder) : base(holder) {}
        
        public override void Subscribe()
        {
            Eventbus.LinkEvents.OnCreatingCombatPairs += SetLinkedActors;
        }
        
        public static void SetLinkedActors(List<uint> actors) //ters de gelebilir
        {
            
            ResetAllLinks();
            var actorsAmount = actors.Count;
            for (var i = 0; i < actorsAmount ; i++)
            {
                var mainID = actors[i];
                var nextIDInOrder = actors[(i + 1) % actorsAmount];
                //Debug.Log(mainID);
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
            Eventbus.LinkEvents.OnCreatingCombatPairs -= SetLinkedActors;
        }

    }
}