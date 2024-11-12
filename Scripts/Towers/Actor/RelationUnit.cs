using System.Collections.Generic;
using System.Linq;
using Towers;
using UnityEngine;

namespace Actor
{
    public class RelationUnit : ActorUnit
    {
        public RelationUnit(ActorHolder holder) : base(holder) {}
        private bool isReversed = false;

        public override void Subscribe()
        {
            Eventbus.ActorEvents.OnReverseRelations += ReverseRelations;
        }

        private void ReverseRelations()
        {
            isReversed = !isReversed;
            List<uint> actors = ActorHolder.Registry.Keys.ToList();
            
            if(isReversed)
                actors.Reverse();
            
            SetRelations(actors);
        }

        public void SetRelations(List<uint> actors)
        {
            SetLinkedActors(actors);
            SetNeighbours(actors);
            
            Eventbus.ActorEvents.OnRelationsSet?.Invoke(actors, isReversed);
        }
        
        private void SetLinkedActors(List<uint> actors) 
        {
            var actorsAmount = actors.Count;
            for (var i = 0; i < actorsAmount ; i++)
            {
                var mainID = actors[i];
                var nextIDInOrder = actors[(i + 1) % actorsAmount];
                //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da
                
                ActorHolder.Registry[mainID].LinkActors(nextIDInOrder); //burda patlar, double'ın elemanı olup registeryde bulunmayabilir!
            }
        }

        private void SetNeighbours(List<uint> actors)
        {
            var actorsAmount = actors.Count;
            for (var i = 0; i < actorsAmount ; i++)
            {
                var mainID = actors[i];
                var mainActor = ActorHolder.Registry[mainID];

                var previousID = i - 1 < 0 ? actors[actorsAmount - 1] : actors[i - 1];
                
                
                var nextID = actors[(i + 1) % actorsAmount];
                
                mainActor.SetNeighbours(previousID, nextID);
                
            }
          
        }
        public override void Unsubscribe()
        {
            Eventbus.ActorEvents.OnReverseRelations -= ReverseRelations;
        }

    }
}