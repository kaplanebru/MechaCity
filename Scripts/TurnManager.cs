using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;


public class TurnTransferData
{
    public List<object> TransferList = new();
}

public class TurnManager : MonoBehaviour
{
    BaseTurnHandler[] turnHandlers;
    
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
        currentTurnHandler.ProcessTransferredData(turnHandlers[turnIndex - 1].DataToTransfer);
    }

}
    