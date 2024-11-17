using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Grid;
using Towers;
using UnityEngine;

namespace Grid
{
    public class RotativeGrid : MonoBehaviour
    {
        public GridData Data;

        private Dictionary<int, ActorData> actorBySlot = new();
        private List<InterruptionByActor> interruptionActors = new();

        private uint[] _actors;
        private bool isReversed = false;
        private GridToIndicator gridToIndicator = new();


        private void OnEnable()
        {
            Eventbus.ActorEvents.OnRegistryUpdate += SetGrid;
            Eventbus.ActorEvents.OnReverseGrid += ReverseTargets;
            gridToIndicator.Subscribe();
        }


        void SetInterruptionActors() //her actor yenilendiğinde
        {
            interruptionActors.Clear();
            foreach (var interruptionSlot in Data.interruptions)
            {
                InterruptionByActor interruptionByActor = new();
                
                interruptionByActor.id = interruptionSlot.id;
                interruptionByActor.Interrupted = actorBySlot[interruptionSlot.Interrupted].ID;
                
                foreach (var slot in interruptionSlot.Interrupters)
                {
                    interruptionByActor.Interrupters.Add(actorBySlot[slot].ID);
                }
                
                interruptionActors.Add(interruptionByActor);
            }
        }

        public bool TryCheckInterruptions(uint[] linkedActors, out uint interruptedActor)
        {
            interruptedActor = 0;

            foreach (var interruption in interruptionActors)
            {
                if(linkedActors.Contains(interruption.Interrupted)) continue;
                if(linkedActors.All(interruption.Interrupters.Contains)) continue;

                interruptedActor = interruption.Interrupted;
                return true;
            }

            return false;
        }


        private void ResolveRelationsFromGrid(
            Func<ActorData, HashSet<uint>> getRelatedActors,
            Func<Slot, int[]> getRelatedSlots)
        {
            foreach (var slot in Data.slots)
            {
                var actor = actorBySlot[slot.Id];
                getRelatedActors(actor).Clear();

                foreach (var relatedSlotId in getRelatedSlots(slot))
                {
                    var relatedActor = actorBySlot[relatedSlotId];
                    if (relatedActor == actor) continue;

                    getRelatedActors(actor).Add(relatedActor.ID);
                }
            }
        }

        void ResolveTargetActors()
        {
            ResolveRelationsFromGrid(
                actor => actor.TargetActors,
                slot => slot.TargetSlots);

            gridToIndicator.SetIndicatorDatas(_actors);
        }

        void ResolveTargetActorsReversed()
        {
            ResolveRelationsFromGrid(
                actor => actor.TargetActors,
                slot => slot.ReversedTargetSlots);

            gridToIndicator.SetIndicatorDatas(_actors);
        }

        void ResolveNeighbours()
        {
            ResolveRelationsFromGrid(
                actor => actor.Neighbours,
                slot => slot.Neighbours);
        }

        private void ReverseTargets()
        {
            isReversed = !isReversed;
            _actors = _actors.Reverse().ToArray();
            SetReversedGrid();
        }

        void SetReversedGrid()
        {
            if (isReversed)
                ResolveTargetActorsReversed();
            else
                ResolveTargetActors();

            SendRelations(isReversed);
        }

        void SetGrid(uint[] actors)
        {
            _actors = actors;
            actorBySlot.Clear();

            FillGridWithActors();
            ResolveTargetActors();
            ResolveNeighbours();
            SetInterruptionActors();

            SendRelations(false);
        }

        void SendRelations(bool reversed)
        {
            Eventbus.ActorEvents.OnRelationsSet?.Invoke(_actors.ToList(), reversed);
        }

        private void FillGridWithActors()
        {
            int i = 0;
            foreach (var actorID in _actors)
            {
                var actor = ActorHolder.Registry[actorID];

                for (var j = 0; j < actor.Towers.Length; j++)
                {
                    actorBySlot.Add(i, actor);
                    i++;
                }
            }
        }

        private void OnDisable()
        {
            Eventbus.ActorEvents.OnRegistryUpdate -= SetGrid;
            Eventbus.ActorEvents.OnReverseGrid -= ReverseTargets;
            gridToIndicator.Unsubscribe();
        }

        //private Dictionary<ActorData, List<int>> slotsByActors = new();
        void GetSlotsByActors()
        {
            // int i = 0;
            // foreach (var actorID in _actors)
            // {
            //     var actor = ActorHolder.Registry[actorID];
            //     slotsByActors.Add(actor, new List<int>());
            //     for (var j = 0; j < actor.Towers.Length; j++)
            //     {
            //         slotsByActors[actor].Add(i);
            //         i++;
            //     }
            // }
        }

        void DebugActors()
        {
            foreach (var id in _actors)
            {
                var actor = ActorHolder.Registry[id];
                foreach (var target in actor.TargetActors)
                {
                    Debug.Log(actor.ID + " target:" + target);
                }
            }
        }
    }
}