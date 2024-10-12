using System.Collections.Generic;
using System.Linq;
using Towers;

namespace Actor
{
    public class RelationController : ActorController
    {
        public RelationController(ActorManager manager) : base(manager) {}
        
        public override void Subscribe()
        {
            Eventbus.LinkEvents.OnCreatingCombatPairs += SetLinkedTowers;
        }

        public void SetRelationData(int id, params int[] towers)
        {
            ActorManager.Registry[id].LinkedActors.Clear();
            ActorManager.Registry[id].LinkedActors.AddRange(towers);
        }

        public void SetLinkedTowers(List<int> towers) //ters de gelebilir
        {
            ResetAllLinks();
            //link de dahil gibi düşün, ters çevirdiğinde dümdüz ters çevirmek sorun olabilir
            for (var i = 0; i < ActorManager.Registry.Count; i++)
            {
                int mainID = ActorManager.Registry.ElementAt(i).Key;
                int nextInOrder = (i + 1) % ActorManager.Registry.Count;
                var next = ActorManager.Registry.ElementAt(nextInOrder).Key;
                
                //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da
                
                ActorManager.Registry[mainID].SetLinkedTowers(next);
            }
            
        }

        private void ResetAllLinks()
        {
            foreach (var registry in ActorManager.Registry.Values)
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