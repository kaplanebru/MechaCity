using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnAction turnAction;

    public Dictionary<string, Team> teams;

    public abstract TurnHandlerType HandlerType { get; }
   
   
    public abstract void OnHandlerEnabled();

    private void OnEnable()
    {
        turnAction = TurnAction.Started;
        OnHandlerEnabled();
    }
    
    public virtual void ProcessIncomingData(BaseTurnData data){}
    
    public abstract void Setup();
    
    public void CompleteAction()
    {
        turnAction = TurnAction.Completed;
        enabled = false;
    }
    public void SetTeams(Dictionary<string, Team> _teams)
    {
        teams = _teams;
    }
    
    public void ActionCompletedByUser()
    {
        Eventbus.NetworkEvents.OnActionCompleteRequestByUser?.Invoke();
        //Eventbus.NetworkEvents.OnActionCompletedByUser?.Invoke();
        //CompleteAction();
    }
    
    public abstract void Unsubscribe();
    private void OnDisable()
    {
        Unsubscribe();
    }

    
}