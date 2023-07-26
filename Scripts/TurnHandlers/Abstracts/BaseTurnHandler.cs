using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnAction turnAction;
    public abstract void Subscribe();
    public abstract void Unsubscribe();
    private void OnEnable()
    {
        turnAction = TurnAction.Started;
        Subscribe();
    }
    private void OnDisable()
    {
        Unsubscribe();
    }

    public void CompleteAction()
    {
        turnAction = TurnAction.Completed;
        enabled = false;
    }
    
    
}
