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
        [ReadOnly] public TeamType TeamType; //gerek olmayabilir, sonuçta tarafsız
        public const int SlotAmount = 3;
        [ReadOnly] public Slot[] Slots = new Slot[SlotAmount];
        private List<Tower> _towers = new();

        public void Initialize(TeamType teamType, List<Tower> towers)
        {
            _towers = towers;
            Setup(teamType);
        }

        void Setup(TeamType teamType)
        {
            TeamType = teamType;
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