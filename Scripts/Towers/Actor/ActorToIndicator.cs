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
            Eventbus.ActorEvents.OnRelationsSet += RestoreActorEdges;
        }

       
        private void RestoreActorEdges(List<uint> actorIDs, bool isReversed)
        {
            EdgesByActors.Clear();
            foreach (var actorID in actorIDs)
            {
                var actor = ActorHolder.Registry[actorID];
                EdgesByActors.Add(actorID, new List<Vector3> { actor.Center});
                
                foreach (var targetActor in actor.TargetActors)
                {
                    EdgesByActors[actorID].Add(ActorHolder.Registry[targetActor].Center);
                    //Debug.Log(actor.ID + " target:" + targetActor);
                   
                }
            }
            GeneralEventbus.IndicatorEvents.OnActorsEdgesRestored?.Invoke(EdgesByActors);
        }

        public void Unsubscribe()
        {
            Eventbus.ActorEvents.OnRelationsSet -= RestoreActorEdges;
        }
    }

}
