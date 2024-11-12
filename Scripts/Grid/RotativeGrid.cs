using System.Collections;
using System.Collections.Generic;
using Actor;
using Grid;
using UnityEngine;

public class RotativeGrid : MonoBehaviour
{
    public GridData Data;
    public Dictionary<ActorData, List<Slot>> GridRegistry = new();
    private Dictionary<int, ActorData> actorsBySlots = new();

    public void FillGridWithActors(List<uint> actors)
    {
        int i = 0;
        while (i < Data.slots.Length)
        {
            var actor = ActorHolder.Registry[actors[i]];
            //GridRegistry.Add(actor, new List<Slot>());

            int j = 0;
            while (j < actor.Towers.Length)
            {
                actorsBySlots.Add(Data.slots[i].Id, actor);
                //Data.slots[i].Actor = actor;
                //GridRegistry[actor].Add(Data.slots[i]);
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
            }
        }
    }
    
}