using System.Collections;
using System.Collections.Generic;
using Actor;
using DG.Tweening;
using Enums;
using Towers;
using UnityEngine;

namespace Actor
{
    public class InterruptionMotion
    {
        private ActorData currentActor;
        private List<Vector3> startPositions = new();

        public void Subscribe()
        {
            Eventbus.LinkEvents.OnInterruptionDetected += MoveOut;
            Eventbus.TurnStateEvents.OnTurnStateBegin += RestoreActorPosition;
        }
        
        void MoveOut(uint actorID, Vector3 offset)
        {
            currentActor = ActorDB.Registry[actorID];
           
            foreach (var tower in currentActor.Towers)
            {
                MoveTower(AllTowers.GetTower(tower.UniqID), offset);
            }
        }

        void MoveTower(Tower tower, Vector3 offset)
        {
            startPositions.Add(tower.transform.localPosition);
            tower.transform.DOLocalMove(tower.transform.localPosition + offset, .5f);
        }

        
        void RestoreActorPosition(TurnStateType stateType)
        {
            if(stateType != TurnStateType.Exit) return;
            if(currentActor == null) return;
            
            for (var i = 0; i < currentActor.Towers.Length; i++)
            {
                var tower = AllTowers.GetTower(currentActor.Towers[i].UniqID);
                tower.transform.DOLocalMove(startPositions[i], .5f);
            }
            
            startPositions.Clear();
            currentActor = null;
        }

        public void Unsubscribe()
        {
            Eventbus.LinkEvents.OnInterruptionDetected -= MoveOut;
            Eventbus.TurnStateEvents.OnTurnStateBegin -= RestoreActorPosition;
        }
    }

}
