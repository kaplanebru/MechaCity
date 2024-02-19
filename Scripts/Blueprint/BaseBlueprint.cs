using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public abstract class BaseBlueprint<TAction> where TAction : IBpAction
{
    public abstract TAction BpAction { get; set; }
    public abstract BpType Type { get; set; }
    
    
    public void TryTakeAction()
    {
        BpAction?.Execute();
    }

   
}