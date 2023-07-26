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
    public List<object> TransferList = new();
}

public class TurnManager : MonoBehaviour
{
    BaseTurnHandler[] turnHandlers;
    public TurnDataHolder _turnDataHolder = new();


    private void OnEnable()
    {
        //Eventbus.TurnEvents.OnTurnActionEnded += GetDatas;
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
            
            GetTransferredData(i, currentTurnHandler);

            yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed);
        }
    }

    void GetTransferredData(int turnIndex, BaseTurnHandler currentTurnHandler)
    {
        if (turnIndex <= 0) return;
        
        Eventbus.TurnEvents.OnTurnActionEnabled?.Invoke(turnHandlers[turnIndex - 1].DataToTransfer);//_turnDataHolder.TurnDataList.Last()
        currentTurnHandler.ProcessTransferredData();
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