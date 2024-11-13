using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Grid;
using Towers;
using UnityEngine;

public class RotativeGrid : MonoBehaviour
{
    public GridData Data;
    private Dictionary<int, ActorData> actorsBySlots = new();
    private uint[] _actors;

    private void OnEnable()
    {
        Eventbus.ActorEvents.OnRegistryUpdate += SetGrid;
    }

    void SetGrid(uint[] actors)
    {
        _actors = actors;
        actorsBySlots.Clear();
        
        FillGridWithActors();
        ResolveTargetActors();
        ResolveNeighbours();
        
        Eventbus.ActorEvents.OnRelationsSet?.Invoke(_actors.ToList(), false);
    }

    void ResolveTargetActors()
    {
        ResolveRelationsFromGrid(
            actor => actor.LinkedActors, 
            slot => slot.RelatedSlots);
    }

    void ResolveTargetActorsReversed()
    {
        ResolveRelationsFromGrid(
            actor => actor.LinkedActors, 
            slot => slot.ReverseRelatedSlots);
    }

    void ResolveNeighbours()
    {
        ResolveRelationsFromGrid(
            actor => actor.Neighbours, 
            slot => slot.Neighbours);
    }
    
    private void ResolveRelationsFromGrid(
        Func<ActorData, List<uint>> getRelatedActors, 
        Func<Slot, int[]> getRelatedSlots)
    {
        foreach (var slot in Data.slots)
        {
            var actor = actorsBySlots[slot.Id];
            getRelatedActors(actor).Clear();
        
            foreach (var relatedSlotId in getRelatedSlots(slot))
            {
                var relatedActor = actorsBySlots[relatedSlotId];
                if (relatedActor == actor) continue;
            
                getRelatedActors(actor).Add(relatedActor.ID);
            }
        }
    }

    public void FillGridWithActors()
    {
        int slot = 0;
        int act = 0;
        while (slot < Data.slots.Length)  //actor sayısı slot sayısından az olabilir
        {
            var actor = ActorHolder.Registry[_actors[act]]; //atlanan actor oluyor

            for (int j = 0; j < actor.Towers.Length; j++)
            {
                actorsBySlots.Add(Data.slots[slot].Id, actor);
                slot++;
            }
            act++;
        }
    }

    private void OnDisable()
    {
        Eventbus.ActorEvents.OnRegistryUpdate -= SetGrid;
    }
}