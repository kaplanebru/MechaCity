using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class GameGrid  //TODO: SO yapılabilir : 2 tane türetirilir
{
    public const int SlotAmount = 3;
    [ReadOnly]public Slot[] Slots = new Slot[SlotAmount];

    public void Initialize(BasePlayer player)
    {
        Eventbus.FireEvents.OnTowerKilled += FindKilledSlot;
        CreateSlots();
        SetSlots(player);
    }

    private void FindKilledSlot(Tower deadTower)
    {
        foreach (var slot in Slots)
        {
            if(slot.Tower != deadTower) continue;

            slot.HasTower = false;
            Eventbus.FireEvents.OnTowerDied?.Invoke(new GridTowerRelationModel(this, deadTower));
        }
    }

    void CreateSlots()
    {
        for (int i = 0; i < SlotAmount; i++)
        {
            Slots[i] = new Slot();
        }
    }
    
    void SetSlots(BasePlayer player)
    {
        for (int i = 0; i < SlotAmount; i++)
        {
            //Slots[i].OnSlotEnabled();
            Slots[i].Number = i;
            Slots[i].Tower = player.Data.Towers[i];
        }
    }

    public void DisableGrid()
    {
        Eventbus.FireEvents.OnTowerKilled -= FindKilledSlot;
        // foreach (var slot in Slots)
        // {
        //     slot.OnSlotDisabled();
        // }
    }


}
