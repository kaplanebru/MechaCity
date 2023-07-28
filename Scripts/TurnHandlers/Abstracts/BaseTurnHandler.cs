using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnAction turnAction;
   
    public abstract void Subscribe();
    public abstract void Unsubscribe();
    
    public virtual void ProcessTransferredData(BaseTurnData data){}

    private void OnEnable()
    {
        turnAction = TurnAction.Started;
        Subscribe();
    }

    public void CompleteAction()
    {
        turnAction = TurnAction.Completed;
        //Eventbus.TurnEvents.OnTurnActionEnded?.Invoke(DataToTransfer);
        enabled = false;
    }
    

    private void OnDisable()
    {
        Unsubscribe();
    }

    
}