using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public TurnState state;
    ITurnHandler[] turnHandlers;

    public List<Tower> currentTowerGroup = new();

    private void OnEnable()
    {
        Eventbus.TurnEvents.OnSelectionEnded += RiseAndFallState;
        
    }

    private void Start()
    {
        turnHandlers = GetComponentsInChildren<ITurnHandler>(true).ToArray();
        state = TurnState.Selection;
        StartCoroutine(nameof(TurnActionChainRoutine));
    }

   
    IEnumerator TurnActionChainRoutine()
    {
       
        foreach (var turnHandler in turnHandlers)
        {
            var oldState = state;
            DisableAllTurnHandlers();
            BaseTurnHandler turnHandlerObject = turnHandler as BaseTurnHandler;
            turnHandlerObject.gameObject.SetActive(true);
            turnHandlerObject.enabled = true;
            
            RaiseTurnStateChangeEvent(this);
            //Eventbus.TurnEvents.OnTurnStateChanged?.Invoke(args); //new object[] { this }


            yield return new WaitUntil(() => state != oldState);
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

    IEnumerator DelayForAWhile(float amount)
    {
        yield return new WaitForSeconds(amount);
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

    
}
