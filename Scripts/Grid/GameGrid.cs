using System;
using System.Collections.Generic;
using Towers;
using Unity.Collections;


namespace Grid
{
    [Serializable]
    public class GameGrid //TODO: SO yapılabilir : 2 tane türetirilir
    {
        //
        // public const int SlotAmount = 3;
        // [ReadOnly] public Slot[] Slots = new Slot[SlotAmount];
        // private List<TowerData> _towers = new();
        //
        // public void Initialize(List<TowerData> towers)
        // {
        //     _towers = towers;
        //     Setup();
        // }
        //
        // void Setup()
        // {
        //     CreateSlots();
        //     SetSlots();
        // }
        //
        //
        // void CreateSlots()
        // {
        //     for (int i = 0; i < SlotAmount; i++)
        //     {
        //         Slots[i] = new Slot {};
        //     }
        // }
        //
        // void SetSlots()
        // {
        //     for (int i = 0; i < SlotAmount; i++)
        //     {
        //         Slots[i].Id = i;
        //         _towers[i].SlotId = i;
        //         Slots[i].Tower = _towers[i];
        //     }
        // }
    }
}