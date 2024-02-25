using System.Collections;
using System.Collections.Generic;
using Enums;
using Turn;
using UnityEngine;

public class ExitState : BaseTurnState
{

    public override int StateId { get; set; }
    //public override TurnStateType StateType => TurnStateType.Exit;
    public override TurnStateType StateType { get; }

  

    public override void Subscribe()
    {
       
    }

  

  

    public override void StartState()
    {
        
    }

    public override void ResetPreviousTurnData()
    {
        
    }

    public override void RestorePreviousSelectionColors()
    {
        
    }

    public override void Unsubscribe()
    {
        
    }
}
