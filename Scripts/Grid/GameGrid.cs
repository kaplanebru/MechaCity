using System;
using System.Collections.Generic;
using Unity.Collections;
using Enums;
using Towers;


namespace Grid
{
    [Serializable]
    public class GameGrid //TODO: SO yapılabilir : 2 tane türetirilir
    {
        
        public const int SlotAmount = 3;
        [ReadOnly] public Slot[] Slots = new Slot[SlotAmount];
        private List<Tower> _towers = new();

        public void Initialize(List<Tower> towers)
        {
            _towers = towers;
            Setup();
        }

        void Setup()
        {
            CreateSlots();
            SetSlots();
        }


        void CreateSlots()
        {
            for (int i = 0; i < SlotAmount; i++)
            {
                Slots[i] = new Slot
                {
                    HasTower = true
                };
            }
        }

        void SetSlots()
        {
            for (int i = 0; i < SlotAmount; i++)
            {
                Slots[i].Id = i;
                Slots[i].Tower = _towers[i];
            }
        }
    }
}