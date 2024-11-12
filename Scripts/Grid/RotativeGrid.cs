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
        ResolveLinkedActorsFromGrid();
    }

    public void FillGridWithActors()
    {
        int i = 0;
        while (i < Data.slots.Length)
        {
            var actor = ActorHolder.Registry[_actors[i]];

            for (int j = 0; j < actor.Towers.Length; j++)
            {
                actorsBySlots.Add(Data.slots[i].Id, actor);
                i++;
            }
        }
    }

    public void ResolveLinkedActorsFromGrid()
    {
        foreach (var slot in Data.slots)
        {
            var actor = actorsBySlots[slot.Id];

            foreach (var relatedSlot in slot.RelatedSlots)
            {
                var relatedActor = actorsBySlots[relatedSlot];
                if(relatedActor == actor) continue;
                
                actor.LinkedActors.Add(relatedActor.ID);
               // Debug.Log("actor: " + actor.ID + " related: " + relatedActor.ID);
            }
        }
        
        Eventbus.ActorEvents.OnRelationsSet?.Invoke(_actors.ToList(), true);
    }

    private void OnDisable()
    {
        Eventbus.ActorEvents.OnRegistryUpdate -= SetGrid;
    }
}