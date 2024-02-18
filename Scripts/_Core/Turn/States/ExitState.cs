using System.Collections;
using System.Collections.Generic;
using Enums;
using Turn;
using UnityEngine;

public class ExitState : BaseTurnState
{
    
    public override int StateId { get; set; }
    public override TurnStateType StateType => TurnStateType.Exit;
  

    public override void Subscribe()
    {
       
    }

  

    public override void UpdateState(TurnManager turnManager)
    {
       
    }

    public override void Setup()
    {
        
    }

    public override void Unsubscribe()
    {
        
    }
}
