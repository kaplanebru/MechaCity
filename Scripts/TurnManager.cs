using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public ITurnActionHandler[] turnHandlers;
    
    private void Start()
    {
        turnHandlers = GetComponentsInChildren<ITurnActionHandler>(true).ToArray();
        StartCoroutine(nameof(TurnActionRoutine));
    }


    IEnumerator TurnActionRoutine()
    {
        for (var i = 0; i < turnHandlers.Length; i++)
        {
            ITurnActionHandler currentTurnHandler = turnHandlers[i] as BaseTurnHandler<BaseTransferData>;
            currentTurnHandler.enabled = true;

            GetTransferredData(i, currentTurnHandler);

            yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed);
        }
    }

    void GetTransferredData(int turnIndex, BaseTurnHandler currentTurnHandler)
    {
        if (turnIndex <= 0) return;
        currentTurnHandler.ProcessTransferredData(turnHandlers[turnIndex - 1].transferData);
    }

}
    