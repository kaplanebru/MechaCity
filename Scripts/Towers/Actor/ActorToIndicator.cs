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
                
                List<Vector3> edgePoints = new();
                edgePoints.Add(actor.Center);
                
                foreach (var linkedActor in actor.LinkedActors)
                {
                    edgePoints.Add(ActorHolder.Registry[linkedActor].Center);
                }
                
                EdgesByActors.Add(actorID, edgePoints);
            }
            GeneralEventbus.IndicatorEvents.OnActorsResolved?.Invoke(EdgesByActors);
        }

        public void Unsubscribe()
        {
            Eventbus.ActorEvents.OnRelationsSet -= ResolveRelations;
        }
    }

}
