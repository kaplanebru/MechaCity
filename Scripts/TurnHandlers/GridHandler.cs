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

    public GameGrid Main;
    public GameGrid Rival;
    public override void Subscribe()
    {
    }
    
    public GridData Data { get; private set; }

    public override void Unsubscribe()
    {
    }

    void MatchTowers() //önce matchle sonra vur
    {
        for (var t = 0; t < GameGrid.ColumnAmount; t++)
        {
            //Main.Columns[t].Lines[t]
            
           
            
           
        }
    }

   
    
    void RestoreRelatedTowerGrid(){}
    
    //bu ikisi restore grid phase'inde yapılabilir

    void SwitchTurn()
    {
        
    }

}
