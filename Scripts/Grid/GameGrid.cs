using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class GameGrid
{
   
    public const int SlotAmount = 3;
    [ReadOnly]public Slot[] Slots = new Slot[SlotAmount];

    public void Initialize()
    {
        EnumerateAllSlots();
    }
    void EnumerateAllSlots()
    {
        for (int i = 0; i < SlotAmount; i++)
        {
            Slots[i].number = i;
        }
    }
}
