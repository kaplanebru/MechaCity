using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnTransferData DataToTransfer = new ();
    protected TurnTransferData TransferredData = new();
    public TurnAction turnAction;
    public abstract void Subscribe();
    public abstract void Unsubscribe();
    
    public virtual void ProcessTransferredData() {}
    private void OnEnable()
    {
        Eventbus.TurnEvents.OnTurnActionEnabled += GetPreviousTurnData;
        turnAction = TurnAction.Started;
        Subscribe();
    }
    
    private void GetPreviousTurnData(TurnTransferData transferredData)
    {
        TransferredData = transferredData;
    }
    
    public void CompleteActionAndTransferData(params object[] args)
    {
        DataToTransfer.TransferList.AddRange(args);
        
        turnAction = TurnAction.Completed;
        Eventbus.TurnEvents.OnTurnActionEnded?.Invoke(DataToTransfer);
        
        enabled = false;
    }
    
    private void OnDisable()
    {
        Eventbus.TurnEvents.OnTurnActionEnabled -= GetPreviousTurnData;
        Unsubscribe();
    }
}
