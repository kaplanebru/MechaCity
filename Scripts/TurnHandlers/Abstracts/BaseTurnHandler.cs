using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnAction turnAction;

    public BasePlayer currentPlayer;
    public BasePlayer rivalPlayer;
   
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

    public void SetPlayers(BasePlayer _currentPlayer, BasePlayer _rivalPlayer)
    {
        currentPlayer = _currentPlayer;
        rivalPlayer = _rivalPlayer;
    }
    

    private void OnDisable()
    {
        Unsubscribe();
    }

    
}