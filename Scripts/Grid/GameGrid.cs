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

    public void Initialize(BasePlayer player) //CALL!!
    {
        CreateSlots();
        SetSlots(player);
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
            Slots[i].OnSlotEnabled();
            Slots[i].Number = i;
            Slots[i].Tower = player.Data.Towers[i];
        }
    }

    public void DisableGrid()
    {
        foreach (var slot in Slots)
        {
            slot.OnSlotDisabled();
        }
    }


}
