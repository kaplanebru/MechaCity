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
        private List<int> _towerIDs = new();

        public void Initialize(List<int> towerIDs)
        {
            _towerIDs = towerIDs;
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
                Slots[i] = new Slot {};
            }
        }

        void SetSlots()
        {
            for (int i = 0; i < SlotAmount; i++)
            {
                Slots[i].Id = i;
                Slots[i].TowerID = _towerIDs[i];
            }
        }
    }
}