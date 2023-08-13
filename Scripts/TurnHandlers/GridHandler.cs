using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;using Datas;
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

    public override void Unsubscribe() {}

    public GridData Data { get; private set; }
    
    //2 taraftan biri kale kaybedince çek edilebilir. Rematch şeklinde.
    //Matches.Clear();

   
}

public interface IMatchable<out TTeamData>
{
    public TTeamData TeamData { get; }

    void SearchMatches(Slot slot, GameGrid otherGrid)
    {
        for (int i = 0; i < GameGrid.SlotAmount; i++)
        {
            if(!slot.hasTower) continue;
            
            int number = slot.number - i;
            if (slot.rivalNumber >= 0)
            {
                if (otherGrid.Slots[number].hasTower)
                {
                    
                    break;
                }
            }

            number = slot.number + i;
            if (number < GameGrid.SlotAmount)
            {
                if (otherGrid.Slots[number].hasTower)
                {
                    Match(slot.number, number);
                    break;
                }
            }
        }
    }


    public Dictionary<int, int> PlayerMatches { get; set; }
    void Match(int number1, int number2)
    {
        
    }
    
}