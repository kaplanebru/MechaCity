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


    void SearchMatches(Slot slot, int number)
    {
        if (number is < 0 or >= GameGrid.SlotAmount) return;

        if (Grids[1].Slots[number].available)
        {
            slot.rivalNumber = number;
            return;
        }

        SearchMatches(slot, number - 1);
        SearchMatches(slot, number + 1);
    }

    void SearchPossibleMatches(Slot slot)
    {
        for (int i = 0; i < GameGrid.SlotAmount; i++)
        {
            slot.rivalNumber = slot.number - i;
            if (slot.rivalNumber >= 0)
            {
                if (Grids[1].Slots[slot.number].available)
                {
                    slot.rivalNumber = slot.number;
                    break;
                }
            }
            
            slot.rivalNumber = slot.number + i;
            if (slot.rivalNumber < GameGrid.SlotAmount)
            {
                if (Grids[1].Slots[slot.number].available)
                {
                    slot.rivalNumber = slot.number;
                    break;
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