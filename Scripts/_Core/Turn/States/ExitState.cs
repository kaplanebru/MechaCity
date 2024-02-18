using System.Collections;
using System.Collections.Generic;
using Enums;
using Turn;
using UnityEngine;

public class ExitState : BaseTurnState
{
    public override TurnHandlerType HandlerType { get; }
    public override int StateId { get; set; }
    public override void OnHandlerEnabled()
    {
        
    }

    public override void Subscribe()
    {
        throw new System.NotImplementedException();
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
