using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public abstract class BaseTurnHandler : MonoBehaviour
{
    public TurnData turnData;
    public TurnAction turnAction;
    public abstract void Subscribe();
    public abstract void Unsubscribe();
    private void OnEnable()
    {
        Eventbus.TurnEvents.OnTurnStateChanged += GetPreviousTurnData;
        turnAction = TurnAction.Started;
        Subscribe();
    }

 
    private void GetPreviousTurnData(TurnDataHolder turnDataHolder)
    {
        var dataList = turnDataHolder.TurnDataList;
        var transferredData = dataList.Last();
        
        
        /*foreach (var data in dataList)
        {
            if((List<Tower>)data == null) continue;
            towerGroup.AddRange((List<Tower>)data);
            break;
        }*/
    }

    private void OnDisable()
    {
        Eventbus.TurnEvents.OnTurnStateChanged -= GetPreviousTurnData;
        Unsubscribe();
    }

    public void CompleteAction(params object[] args)
    {
        turnData.Add(args);
        turnAction = TurnAction.Completed;
        Eventbus.TurnEvents.OnTurnActionEnded?.Invoke(turnData);
        enabled = false;
    }
    
    
}
