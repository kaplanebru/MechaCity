using System;
using System.Collections.Generic;
using Unity.Collections;
using Enums;
using Towers;


namespace Grid
{
    [Serializable]
    public class GameGrid  //TODO: SO yapılabilir : 2 tane türetirilir
    {
        [ReadOnly]public TeamType TeamType;
        public const int SlotAmount = 3;
        [ReadOnly]public Slot[] Slots = new Slot[SlotAmount];

        public void Initialize(TeamType teamType, List<Tower> towers)
        {
            Setup(teamType, towers);
        }

        void Setup(TeamType teamType, List<Tower> towers)
        {
            TeamType = teamType;
            CreateSlots();
            SetSlots(towers);
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
    
        void SetSlots(List<Tower> towers)
        {
            for (int i = 0; i < SlotAmount; i++)
            {
                Slots[i].Id = i;
                Slots[i].Tower = towers[i];
            }
        }
    }

}
