using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnActionState turnActionState;
    public abstract void Subscribe();
    public abstract void Unsubscribe();
    private void OnEnable()
    {
        turnActionState = TurnActionState.Started;
        Subscribe();
    }
    private void OnDisable()
    {
        turnActionState = TurnActionState.Completed;
        Unsubscribe();
    }
    
    
    
}
