using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;using Datas;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GridData : BaseTurnData //bizim sonrakine göndereceğimiz
{
    public List<Tower> MainTowers = new();
    public List<Tower> RivalTowers = new();
    
    public GameGridModel[] Grids = new GameGridModel[2];
}

public class MatchHandler : BaseTurnHandler, ITurnActionHandler<GridData>
{
    GridData gridData;
    public override void Subscribe()
    {
        foreach (var grid in gridData.Grids)
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
    public Dictionary<int, int> Matches { get; set; }

    void GetTargets()
    {
        // foreach (var VARIABLE in COLLECTION)
        // {
        //     
        // }
    }

    void SetTarget(Slot slot, GameGridModel otherGridModel)
    {
        for (int i = 0; i < GameGridModel.SlotAmount; i++)
        {
            if(!slot.hasTower) continue;
            
            int number = slot.number - i;
            if (slot.rivalNumber >= 0)
            {
                if (otherGridModel.Slots[number].hasTower)
                {
                    Match(slot.number, number);
                    break;
                }
            }

            number = slot.number + i;
            if (number < GameGridModel.SlotAmount)
            {
                if (otherGridModel.Slots[number].hasTower)
                {
                    Match(slot.number, number);
                    break;
                }
            }
        }
    }


    
    void Match(int number1, int number2)
    {
        
    }
    
}