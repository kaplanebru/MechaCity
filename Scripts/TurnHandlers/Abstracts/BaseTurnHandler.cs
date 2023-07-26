using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnTransferData DataToTransfer;
    public TurnTransferData TransferredData;
    public TurnAction turnAction;
    public abstract void Subscribe();
    public abstract void Unsubscribe();
    private void OnEnable()
    {
        Eventbus.TurnEvents.OnTurnStateChanged += GetPreviousTurnData;
        turnAction = TurnAction.Started;
        Subscribe();
    }
    
    private void GetPreviousTurnData(TurnTransferData transferredData)
    {
        TransferredData = transferredData;
    }

    public virtual void ProcessTransferredData()
    {
    }
    public void CompleteActionAndTransferData(params object[] args)
    {
        DataToTransfer.DataList.Add(args);
        
        turnAction = TurnAction.Completed;
        Eventbus.TurnEvents.OnTurnActionEnded?.Invoke(DataToTransfer);
        
        enabled = false;
    }
    
    private void OnDisable()
    {
        Eventbus.TurnEvents.OnTurnStateChanged -= GetPreviousTurnData;
        Unsubscribe();
    }
}
