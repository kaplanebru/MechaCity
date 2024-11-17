using System.Collections;
using System.Collections.Generic;
using Actor;
using DG.Tweening;
using Towers;
using UnityEngine;

namespace Actor
{
    public class InterruptionMotion
    {
        private ActorData currentActor;
        private List<Vector3> startPositions;

        public void Subscribe()
        {
            Eventbus.LinkEvents.OnInterruptionDetected += MoveOut;
        }
        
        void MoveOut(uint actorID, Vector3 offset)
        {
            var actor = ActorHolder.Registry[actorID];
            foreach (var tower in actor.Towers)
            {
                MoveOutTower(AllTowers.GetTower(tower.UniqID), offset);
            }
        }

        void MoveOutTower(Tower tower, Vector3 offset)
        {
            tower.transform.DOLocalMove(tower.transform.localPosition + offset, .5f);
        }

        public void Unsubscribe()
        {
            Eventbus.LinkEvents.OnInterruptionDetected -= MoveOut;
        }
    }

}
