using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class TurnDataHolder
{
    public List<TurnTransferData> TurnDataList = new();
    //public List<Tower> currentTowerGroup = new();
}

public class TurnTransferData
{
    public List<object> DataList = new();
}

public class TurnManager : MonoBehaviour
{
    BaseTurnHandler[] turnHandlers;
    public TurnDataHolder _turnDataHolder = new();

   
    private void OnEnable()
    {
       Eventbus.TurnEvents.OnTurnActionEnded += GetDatas;
    }

    private void Start()
    {
        turnHandlers = GetComponentsInChildren<BaseTurnHandler>(true).ToArray();
        StartCoroutine(nameof(TurnActionRoutine));
    }

   
    IEnumerator TurnActionRoutine()
    {
        for (var i = 0; i < turnHandlers.Length; i++)
        {
            BaseTurnHandler currentTurnHandler = turnHandlers[i];
            currentTurnHandler.enabled = true;
            GetTransferredData(i);
            currentTurnHandler.ProcessTransferredData();

            yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed);
        }
    }

    void GetTransferredData(int turnIndex)
    {
        if(turnIndex>0)
            Eventbus.TurnEvents.OnTurnStateChanged?.Invoke(turnHandlers[turnIndex-1].DataToTransfer);
    }

    private void GetDatas(TurnTransferData turnTransferData)
    {
       _turnDataHolder.TurnDataList.Add(turnTransferData);
    }
    

    private void OnDisable()
    {
         Eventbus.TurnEvents.OnTurnActionEnded -= GetDatas;
    }
}
