using System.Collections.Generic;
using Towers;

namespace Actor
{
    public class RelationController : ActorController
    {
        public RelationController(ActorManager manager) : base(manager) {}
        
        public override void Subscribe()
        {
            Eventbus.LinkEvents.OnCreatingCombatPairs += SetLinkedTowersForAll;
        }

        public void SetRelationData(int id, params int[] towers)
        {
            ActorManager.Registry[id].LinkedActors.Clear();
            ActorManager.Registry[id].LinkedActors.AddRange(towers);
        }

        public static void SetLinkedTowersForAll(List<int> tempRegistryIDs) //ters de gelebilir List<int> towers
        {
            ResetAllLinks();
            //link de dahil gibi düşün, ters çevirdiğinde dümdüz ters çevirmek sorun olabilir
            for (var i = 0; i < AllTowers.TowersCount; i++)
            {
                var mainID = tempRegistryIDs[i]; //AllTowers.GetData(towers[i]).UniqID;
                int nextInOrder = (i + 1) % tempRegistryIDs.Count;
                int nextID = tempRegistryIDs[nextInOrder];
                //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da
                
                //bu noktada mainID ya da nextID bir tower id olmayabilir. LinkedActor olur.
                
                ActorManager.Registry[mainID].SetLinkedActors(nextID);
            }
        }
        
        /*for (var i = 0; i < tempRegistryIDs; i++)
        {
            int mainID = tempRegistryIDs[i];//ActorManager.Registry.ElementAt(i).Key;
            int nextInOrder = (i + 1) % tempRegistryIDs.Count;
            var next = //ActorManager.Registry.ElementAt(nextInOrder).Key;
                
                //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da
                
                ActorManager.Registry[mainID].SetLinkedTowers(next);
        }*/

        private static void ResetAllLinks()
        {
            foreach (var registry in ActorManager.Registry.Values)
            {
                registry.LinkedActors.Clear();
            }
        }

        public override void Unsubscribe()
        {
            Eventbus.LinkEvents.OnCreatingCombatPairs -= SetLinkedTowersForAll;
        }

    }
}