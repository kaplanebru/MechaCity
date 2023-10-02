using System;
using Unity.Collections;
using Teams;
using Enums;


namespace Grid
{
    [Serializable]
    public class GameGrid  //TODO: SO yapılabilir : 2 tane türetirilir
    {
        [ReadOnly]public TeamType TeamType;
        public const int SlotAmount = 3;
        [ReadOnly]public Slot[] Slots = new Slot[SlotAmount];

        public void Initialize(Team team)
        {
            Setup(team);
        }

        void Setup(Team team)
        {
            TeamType = team.Data.TeamTowerData.TeamType;
            CreateSlots();
            SetSlots(team);
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
    
        void SetSlots(Team team)
        {
            for (int i = 0; i < SlotAmount; i++)
            {
                Slots[i].Id = i;
                Slots[i].Tower = team.Data.Towers[i];
            }
        }
    }

}
