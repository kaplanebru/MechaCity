using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    BaseTurnHandler[] turnHandlers;

   
    [SerializeField]private BasePlayer currentPlayer;
    [SerializeField]private BasePlayer rivalPlayer;

    private void Start()
    {
        turnHandlers = GetComponentsInChildren<BaseTurnHandler>(true).ToArray();
        DisableAllTurnHandlers();
        InitializePlayers();
        StartCoroutine(nameof(TurnActionRoutine));
    }

    void DisableAllTurnHandlers()
    {
        foreach (var turnHandler in turnHandlers)
        {
            turnHandler.enabled = false;
        }
    }
    void InitializePlayers()
    {
        currentPlayer.Initialize();
        rivalPlayer.Initialize();
        InitializeFirstMatches();
    }
    
    IEnumerator TurnActionRoutine()
    {
        for (var i = 0; i < turnHandlers.Length; i++)
        {
            BaseTurnHandler currentTurnHandler = turnHandlers[i];
            currentTurnHandler.enabled = true;
            
            currentTurnHandler.SetPlayers(currentPlayer, rivalPlayer);

            GetTransferredData(i, currentTurnHandler);
            currentTurnHandler.Setup();

            yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed);
        }
    }
    
    void GetTransferredData(int turnIndex, BaseTurnHandler currentTurnHandler)
    {
        if (turnIndex <= 0) return;
        
        var transferData = ((ITurnActionHandler<BaseTurnData>)turnHandlers[turnIndex - 1]).Data;
        currentTurnHandler.ProcessTransferredData(transferData);
    }
    
    void InitializeFirstMatches() //Temporary
    {
        for (int i = 0; i < currentPlayer.Data.Towers.Count; i++)
        {
            currentPlayer.Data.Towers[i].Data.LinkedTowers.Add(rivalPlayer.Data.Towers[i]);
            rivalPlayer.Data.Towers[i].Data.LinkedTowers.Add(currentPlayer.Data.Towers[i]);
        }
    }

    void SwitchPlayers()
    {
        (currentPlayer, rivalPlayer) = (rivalPlayer, currentPlayer);
        
        // var temp = currentPlayer;
        // currentPlayer = rivalPlayer;
        // rivalPlayer = temp;
    }

}
    