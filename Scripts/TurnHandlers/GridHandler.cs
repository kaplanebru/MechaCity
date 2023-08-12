using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GridData : BaseTurnData
{
    public List<Tower> MainTowers = new();
    public List<Tower> RivalTowers = new();
}

public class GridHandler : BaseTurnHandler, ITurnActionHandler<GridData>
{
    public GameGrid[] Grids = new GameGrid[2];

    public override void Subscribe()
    {
        foreach (var grid in Grids)
        {
            grid.Initialize();
        }
    }

    public override void Unsubscribe()
    {
    }

    public GridData Data { get; private set; }

    void MatchSlots(GameGrid otherGrid, int slotNumber, Slot slot)
    {
        if (slotNumber < 0 || slotNumber == GameGrid.SlotAmount) return;
        if (otherGrid.Slots[slotNumber].available)
            slot.rivalSlotNumber = slotNumber;
        else
        {
            MatchSlots(otherGrid, slotNumber-1, slot);
            MatchSlots(otherGrid, slotNumber+1, slot);
        }
    }

    void MatchTowers()
    {
        for (int i = 0; i < GameGrid.SlotAmount; i++)
        {
            if (!Grids[0].Slots[i].available) continue;
            if (Grids[1].Slots[i].available)
            {
                Fight();
            }
            else
            {
                if (i > 0 && Grids[1].Slots[i - 1].available)
                {
                    Fight();
                    continue;
                }

                if (i < GameGrid.SlotAmount - 1 && Grids[1].Slots[i + 1].available)
                {
                    Fight();
                }
            }
        }
    }

    void Fight()
    {
    }


    //bu ikisi restore grid phase'inde yapılabilir

    void SwitchTurn()
    {
    }
}