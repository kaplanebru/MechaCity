using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridData : BaseTurnData
{
    public List<Tower> MainTowers = new();
    public List<Tower> RivalTowers = new();
}
public class GridHandler : BaseTurnHandler, ITurnActionHandler<GridData>
{

    public GameGrid MainGrid;
    public GameGrid RivalGrid;
    public override void Subscribe()
    {
        MainGrid.Initialize();
        RivalGrid.Initialize();
    }
    
    public override void Unsubscribe()
    {
    }
    
    public GridData Data { get; private set; }

    void MatchTowers()
    {
        for (int i = 0; i < GameGrid.SlotAmount; i++)
        {
            if(!MainGrid.Slots[i].available) continue;
            if (RivalGrid.Slots[i].available)
            {
                Fight();
            }
            else
            {
                if (i > 0 && RivalGrid.Slots[i - 1].available)
                {
                    Fight();
                    continue;
                }
                
                if(i < GameGrid.SlotAmount-1 && RivalGrid.Slots[i + 1].available)
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
