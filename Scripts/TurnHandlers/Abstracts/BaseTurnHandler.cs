using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnAction turnAction;

    public Dictionary<string, BasePlayer> players;
   
   
    public abstract void OnHandlerEnabled();
    
    private void OnEnable()
    {
        turnAction = TurnAction.Started;
        OnHandlerEnabled();
    }
    
    public virtual void ProcessTransferredData(BaseTurnData data){}
    
    public abstract void Setup();
    
    public void CompleteAction()
    {
        turnAction = TurnAction.Completed;
        //Eventbus.TurnEvents.OnTurnActionEnded?.Invoke(DataToTransfer);
        enabled = false;
    }
    public void SetPlayers(Dictionary<string, BasePlayer> _players)
    {
        players = _players;
    }
    
    public abstract void Unsubscribe();
    private void OnDisable()
    {
        Unsubscribe();
    }

    
}