using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnTransferData DataToTransfer = new ();
    
    public TurnAction turnAction;
    public abstract void Subscribe();
    public abstract void Unsubscribe();
    
    public virtual void ProcessTransferredData(params object[] transferredData) {}
    private void OnEnable()
    {
        turnAction = TurnAction.Started;
        Subscribe();
    }
    
    
    
    public void CompleteActionAndTransferData(params object[] args)
    {
        DataToTransfer.TransferList.AddRange(args);
        
        turnAction = TurnAction.Completed;
        //Eventbus.TurnEvents.OnTurnActionEnded?.Invoke(DataToTransfer);
        
        enabled = false;
    }
    
    private void OnDisable()
    {
        Unsubscribe();
    }
}
