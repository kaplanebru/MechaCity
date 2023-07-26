using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public abstract class BaseTransferData {}

public abstract class BaseTurnHandler<TBaseTransferData> : MonoBehaviour
{
    public TurnAction turnAction;
    public TBaseTransferData transferData;
    public abstract void Subscribe();
    public abstract void Unsubscribe();

    public virtual void ProcessTransferredData(params object[] transferredData) {}
    public virtual void SetTransferData(){}

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