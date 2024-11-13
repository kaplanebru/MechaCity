using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class ActorToIndicator
    {
        private Dictionary<uint, List<Vector3>> EdgesByActors = new();
        public void Subscribe()
        {
            Eventbus.ActorEvents.OnRelationsSet += ResolveRelations;
        }

       
        private void ResolveRelations(List<uint> actorIDs, bool isReversed)
        {
            EdgesByActors.Clear();
            foreach (var actorID in actorIDs)
            {
                var actor = ActorHolder.Registry[actorID];
                EdgesByActors.Add(actorID, new List<Vector3> { actor.Center});
                
                foreach (var targetActor in actor.TargetActors)
                {
                    EdgesByActors[actorID].Add(ActorHolder.Registry[targetActor].Center);
                }
            }
            GeneralEventbus.IndicatorEvents.OnActorsResolved?.Invoke(EdgesByActors);
        }

        public void Unsubscribe()
        {
            Eventbus.ActorEvents.OnRelationsSet -= ResolveRelations;
        }
    }

}
