using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using DG.Tweening;
using Grid;
using Towers;
using UnityEngine;

namespace Grid
{
    public class RotativeGrid : MonoBehaviour
    {
        public GridData Data;

        public static Dictionary<int, ActorData> actorBySlot = new();

        private uint[] _actors;
        private bool isReversed = false;
        private GridToIndicator gridToIndicator = new();
        private InterruptionController interruptionController;


        private void OnEnable()
        {
            interruptionController = new InterruptionController(Data);
            Eventbus.ActorEvents.OnRegistryUpdate += RegisterActors;
            Eventbus.ActorEvents.OnReverseGrid += ReverseTargets;
            Eventbus.LinkEvents.OnLinkActorsLoaded += interruptionController.TryCheckInterruptions;
            gridToIndicator.Subscribe();
        }

        void RegisterActors(uint[] actors)
        {
            _actors = actors;
            actorBySlot.Clear();

            AddMainActors();
            ResolveTargetActors();
            ResolveNeighbours();
            interruptionController.SetInterruptionActors();

            SendGridRegistry(false); //TODO SEPARATE
            GeneralEventbus.InitializerEvents.OnActorsRegisteredToGrid?.Invoke();
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

        void SendGridRegistry(bool reversed)
        {
            Eventbus.ActorEvents.OnGridRegistrySet?.Invoke(_actors.ToList(), reversed);
        }

        private void AddMainActors()
        {
            int i = 0;
            foreach (var actorID in _actors)
            {
                var actor = ActorDB.Registry[actorID];

                for (var j = 0; j < actor.TowerAmount; j++)
                {
                    actorBySlot.Add(i, actor);
                    i++;
                }
            }
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

            SendGridRegistry(isReversed);
        }
        private void OnDisable()
        {
            Eventbus.ActorEvents.OnRegistryUpdate -= RegisterActors;
            Eventbus.ActorEvents.OnReverseGrid -= ReverseTargets;
            Eventbus.LinkEvents.OnLinkActorsLoaded -= interruptionController.TryCheckInterruptions;
            gridToIndicator.Unsubscribe();
        }

      

        void DebugActors()
        {
            foreach (var id in _actors)
            {
                var actor = ActorDB.Registry[id];
                foreach (var target in actor.TargetActors)
                {
                    Debug.Log(actor.ID + " target:" + target);
                }
            }
        }
    }
}