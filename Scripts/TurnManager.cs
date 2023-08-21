using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    BaseTurnHandler[] turnHandlers;
    Dictionary<string, BasePlayer> players;

    //aşağıdakileri asset holdera koy
    [SerializeField]private BasePlayer currentPlayer;
    [SerializeField]private BasePlayer rivalPlayer;
    
    private void Start()
    {
        SetPlayers();
        turnHandlers = GetComponentsInChildren<BaseTurnHandler>(true).ToArray();
        DisableAllTurnHandlers();
        InitializePlayers();
        StartCoroutine(nameof(TurnActionRoutine));
    }

    void SetPlayers()
    {
        players = new Dictionary<string, BasePlayer>()
        {
            {"currentPlayer", currentPlayer},
            {"rivalPlayer", rivalPlayer},
        };
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
        foreach (var player in players)
        {
            player.Value.Initialize();
        }

        SetFirstMatches();
    }
    
    IEnumerator TurnActionRoutine()
    {
        for (var i = 0; i < turnHandlers.Length; i++)
        {
            BaseTurnHandler currentTurnHandler = turnHandlers[i];
            currentTurnHandler.enabled = true;
            
            currentTurnHandler.SetPlayers(players);

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
    
    void SetFirstMatches() //Temporary
    {
        players[nameof(currentPlayer)].LinkFirstMatches(players[nameof(rivalPlayer)]);
        players[nameof(rivalPlayer)].LinkFirstMatches(players[nameof(currentPlayer)]);
    }

    void SwitchPlayers()
    {
        //(currentPlayer, rivalPlayer) = (rivalPlayer, currentPlayer);

        (players[nameof(currentPlayer)], players[nameof(rivalPlayer)]) =
            (players[nameof(rivalPlayer)], players[nameof(currentPlayer)]);
        

        // var temp = currentPlayer;
        // currentPlayer = rivalPlayer;
        // rivalPlayer = temp;
    }
}
    