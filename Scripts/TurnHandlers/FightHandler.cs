using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightData : BaseTurnData
{
   
}
public class FightHandler : BaseTurnHandler, ITurnActionHandler<FightData>
{
    public override void Subscribe()
    {
    }
    
    public FightData Data { get; private set; }
    public override void Unsubscribe()
    {
    }
    
    void GetPositionsOrRelatedTowerGrid(){}
    
    void RestoreRelatedTowerGrid(){}
    
    //bu ikisi restore grid phase'inde yapılabilir

    void SwitchTurn()
    {
        
    }

}
