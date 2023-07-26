using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public abstract class BaseTurnData {}

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnAction turnAction;
   
    public abstract void Subscribe();
    public abstract void Unsubscribe();

    public virtual void SetTransferData(){}
    
    public virtual void ProcessTransferredData(BaseTurnHandler arg){}

    private void OnEnable()
    {
        turnAction = TurnAction.Started;
        Subscribe();
    }


    public void CompleteAction()
    {
        SetTransferData();

        turnAction = TurnAction.Completed;
        //Eventbus.TurnEvents.OnTurnActionEnded?.Invoke(DataToTransfer);

        enabled = false;
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    
}