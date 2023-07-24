using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    ITurnActionHandler[] turnHandlers;

    public List<Tower> currentTowerGroup = new();

    private void OnEnable()
    {
       Eventbus.TurnEvents.OnSelectionEnded += GetTowers;
        
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
            RaiseTurnActionChangeEvent(this);


            yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed);
        }
    }

    void RaiseTurnActionChangeEvent(params object[] args)
    {
        Eventbus.TurnEvents.OnTurnStateChanged?.Invoke(args);
    }
    private void GetTowers(List<Tower> towers)
    {
        currentTowerGroup = towers;
    }
    

    private void OnDisable()
    {
         Eventbus.TurnEvents.OnSelectionEnded -= GetTowers;
    }
}
