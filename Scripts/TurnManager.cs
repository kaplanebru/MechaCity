using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class TurnDataHolder
{
    public List<TurnData> TurnDataList = new();
    //public List<Tower> currentTowerGroup = new();
}

public class TurnData
{
    
}

public class TurnManager : MonoBehaviour
{
    ITurnActionHandler[] turnHandlers;
    public TurnDataHolder _turnDataHolder;

   
    private void OnEnable()
    {
       Eventbus.TurnEvents.OnTurnActionEnded += GetDatas;
        
    }

    private void Start()
    {
        turnHandlers = GetComponentsInChildren<ITurnActionHandler>(true).ToArray();
        StartCoroutine(nameof(TurnActionRoutine));
    }

   
    IEnumerator TurnActionRoutine()
    {
       
        foreach (var turnHandler in turnHandlers)
        {
            BaseTurnHandler currentTurnHandler = turnHandler as BaseTurnHandler;
            currentTurnHandler.enabled = true;
            Eventbus.TurnEvents.OnTurnStateChanged?.Invoke(_turnDataHolder);

            yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed);
        }
    }

    private void GetDatas(TurnData turnData)
    {
       _turnDataHolder.TurnDataList.Add(turnData);
    }
    

    private void OnDisable()
    {
         Eventbus.TurnEvents.OnTurnActionEnded -= GetDatas;
    }
}
