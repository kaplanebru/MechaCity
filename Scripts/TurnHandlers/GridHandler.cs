using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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


    private int i = 0;
   
    
    // void SearchMatches(Slot slot, int pole, int i) // i=1
    // {
    //     if (slot.rivalNumber is < 0 or >= GameGrid.SlotAmount) return; bu iki caseden biri olursa diğerini aramayı bırakıyor
    //
    //     if (Grids[1].Slots[slot.rivalNumber].available) return;
    //     
    //     slot.rivalNumber += i * pole;
    //     pole *= -1;
    //     SearchMatches(slot, pole, i++);
    //     
    // }
    
    //rivalnumber'a da çevrilebilir

    void SearchPossibleMatches(Slot slot, GameGrid otherGrid) //bundaki sorun hiç rival yoksa ortaya çıkıyordu, son rivalNumber'a ateş etmek şeklinde
                                                                //hasrival diye dict yapılabilirdi
    {
        for (int i = 0; i < GameGrid.SlotAmount; i++)
        {
            int rivalNumber = slot.number - i;
            if (slot.rivalNumber >= 0)
            {
                if (otherGrid.Slots[rivalNumber].available)
                {
                    Match(slot.number, rivalNumber);
                    break;
                }
            }

            rivalNumber = slot.number + i;
            if (rivalNumber < GameGrid.SlotAmount)
            {
                if (otherGrid.Slots[rivalNumber].available)
                {
                    Match(slot.number, rivalNumber);
                    break;
                }
            }
        }
    }
    
    //2 taraftan biri kale kaybedince çek edilebilir. Rematch şeklinde.

    void Match(int number1, int number2)
    {
    }


    //bu ikisi restore grid phase'inde yapılabilir

    void SwitchTurn()
    {
    }
}