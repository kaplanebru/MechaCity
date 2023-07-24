using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public TurnState state;
    ITurnActionHandler[] turnHandlers;

    public List<Tower> currentTowerGroup = new();

    private void OnEnable()
    {
       Eventbus.TurnEvents.OnSelectionEnded += RiseAndFallState;
        
    }

    private void Start()
    {
        turnHandlers = GetComponentsInChildren<ITurnActionHandler>(true).ToArray();
        state = TurnState.Selection;
        StartCoroutine(nameof(TurnActionRoutine));
    }

   
    IEnumerator TurnActionRoutine()
    {
       
        foreach (var turnHandler in turnHandlers)
        {
            DisableAllTurnHandlers();
            
            BaseTurnHandler turnHandlerObject = turnHandler as BaseTurnHandler;
            turnHandlerObject.gameObject.SetActive(true);
            turnHandlerObject.enabled = true;
            RaiseTurnStateChangeEvent(this);


            yield return new WaitUntil(() => turnHandlerObject.turnActionState == TurnActionState.Completed);
        }
    }

    void RaiseTurnStateChangeEvent(params object[] args)
    {
        Eventbus.TurnEvents.OnTurnStateChanged?.Invoke(args);
    }
    private void RiseAndFallState(List<Tower> towers)
    {
        currentTowerGroup = towers;
        state = TurnState.RiseAndFallState;
    }
    

    void DisableAllTurnHandlers()
    {
        foreach (var turnHandler in turnHandlers)
        {
            BaseTurnHandler turnHandlerObject = turnHandler as BaseTurnHandler;
            turnHandlerObject.enabled = false;
            turnHandlerObject.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
         Eventbus.TurnEvents.OnSelectionEnded -= RiseAndFallState;
    }
}
