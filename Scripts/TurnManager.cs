using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Unity.Netcode;
using UnityEngine;

public class TurnManager : MonoBehaviour ////NetworkBehaviour
{
    BaseTurnHandler[] turnHandlers;
    Dictionary<string, Team> teams;

    //aşağıdakileri asset holdera koy
    [SerializeField]private Team currentTeam;
    [SerializeField]private Team rivalTeam;
    
    public TurnNetworkHandler turnNetworkHandler;
    
    private void Start()
    {
        SetTeams();
        turnHandlers = GetComponentsInChildren<BaseTurnHandler>(true).ToArray();
        DisableAllTurnHandlers();
        InitializeTeams();
        Eventbus.NetworkEvents.OnTurnHandleTypeChanged += CompleteCurrentAction;
        Eventbus.NetworkEvents.OnNewTurn += NewTurn;
        Play();
    }

    public void Play() //UI kısmına taşı
    {
        StartCoroutine(nameof(TurnActionRoutine));
    }

    void SetTeams()
    {
        teams = new Dictionary<string, Team>()
        {
            {"currentTeam", currentTeam},
            {"rivalTeam", rivalTeam},
        };
    }

    void DisableAllTurnHandlers()
    {
        foreach (var turnHandler in turnHandlers)
        {
            turnHandler.enabled = false;
        }
    }
    void InitializeTeams()
    {
        foreach (var team in teams)
        {
            team.Value.Initialize();
        }
        
        SetFirstMatches();
    }

    void CompleteCurrentAction()
    {
        currentTurnHandler.turnAction = TurnAction.Completed;
        currentTurnHandler.enabled = false;
    }

    private BaseTurnHandler currentTurnHandler;
    IEnumerator TurnActionRoutine()
    {
        for (var i = 0; i < turnHandlers.Length; i++)
        {
            Eventbus.NetworkEvents.OnTurnHandlerEnding?.Invoke(turnHandlers[i].HandlerType); //For MP
            
            currentTurnHandler = turnHandlers[i];
            currentTurnHandler.enabled = true;
            
            currentTurnHandler.SetTeams(teams);

            GetIncomingData(i, currentTurnHandler);
            currentTurnHandler.Setup();

            yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed); //TODO: bool Network variable yapılabilir. Tıklayınca complete oluyor.

            // if (i + 1 < turnHandlers.Length)
            //     Eventbus.NetworkEvents.OnTurnHandlerEnding?.Invoke(turnHandlers[i + 1].HandlerType);
            
        }
        
        Eventbus.TurnEvents.OnTurnCompleted?.Invoke();
    }
    
    void GetIncomingData(int turnIndex, BaseTurnHandler currentTurnHandler)
    {
        if (turnIndex <= 0) return;
        
        var transferData = ((ITurnActionHandler<BaseTurnData>)turnHandlers[turnIndex - 1]).Data;
        currentTurnHandler.ProcessIncomingData(transferData);
    }

    void NewTurn()
    {
        print("new turn");
        StopCoroutine(nameof(TurnActionRoutine));
        
        SwitchTeams();

        StartCoroutine(nameof(TurnActionRoutine));
    }
    void SwitchTeams()
    {
        //(currentTeam, rivalTeam) = (rivalTeam, currentTeam);

        (teams[nameof(currentTeam)], teams[nameof(rivalTeam)]) =
            (teams[nameof(rivalTeam)], teams[nameof(currentTeam)]);

        // var temp = currentTeam;
        // currentTeam = rivalTeam;
        // rivalTeam = temp;
    }
    void SetFirstMatches() //Temporary
    {
        teams[nameof(currentTeam)].LinkFirstMatches(teams[nameof(rivalTeam)]);
        teams[nameof(rivalTeam)].LinkFirstMatches(teams[nameof(currentTeam)]);
    }

    private void OnDisable()
    {
        Eventbus.NetworkEvents.OnTurnHandleTypeChanged -= CompleteCurrentAction;
    }
}
    