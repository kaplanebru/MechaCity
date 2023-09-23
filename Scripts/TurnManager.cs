using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Unity.Netcode;
using UnityEngine;

public class TurnManager : MonoBehaviour ////NetworkBehaviour
{
    public NetworkVariable<TurnHandlerType> turnHandlerType = new(TurnHandlerType.Selection);
    BaseTurnHandler[] turnHandlers;
    Dictionary<string, Team> turnTeams;
    [SerializeField] private TeamsHandler teamsHandler;
    public TeamType currentTeamType = TeamType.Team1;
    
    private BaseTurnHandler currentTurnHandler;


    private void OnEnable()
    {
        Eventbus.NetworkEvents.OnAllClientsSet += FirstTurn;
        Eventbus.NetworkRequestEvents.OnCompleteActionRequest += CompleteActionByUser;
        Eventbus.NetworkRequestEvents.OnNewTurnRequest += NewTurn;
        
        turnHandlers = GetComponentsInChildren<BaseTurnHandler>(true).ToArray();
        DisableAllTurnHandlers();
    }

    private void Initialize()
    {
        SetTurnTeams();
        Eventbus.TurnEvents.OnInitialize?.Invoke();
    }
    

    void SetTurnTeams()
    {
        turnTeams = new Dictionary<string, Team>()
        {
            {"currentTeam", teamsHandler.teams[0]},
            {"rivalTeam", teamsHandler.teams[1]},
        };
    }

    void DisableAllTurnHandlers()
    {
        foreach (var turnHandler in turnHandlers)
        {
            turnHandler.enabled = false;
        }
    }
    
    
    public void FirstTurn(Team[] teams)
    {
        Initialize();
        StartCoroutine(nameof(TurnActionRoutine));
    }
    
    IEnumerator TurnActionRoutine()
    {
        Eventbus.TurnEvents.OnTurnStarted?.Invoke(currentTeamType);
        
        
        for (var i = 0; i < turnHandlers.Length; i++)
        {
            currentTurnHandler = turnHandlers[i];
            currentTurnHandler.enabled = true;
            currentTurnHandler.SetTeams(turnTeams);

            GetIncomingData(i);
            currentTurnHandler.Setup();

            yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed);
        }

        if(!GameEnding())
            Eventbus.TurnEvents.OnTurnEnding?.Invoke();
    }

    void GetIncomingData(int turnIndex)
    {
        if (turnIndex <= 0) return;

        var transferData = ((ITurnActionHandler<BaseTurnData>) turnHandlers[turnIndex - 1]).Data;
        currentTurnHandler.ProcessIncomingData(transferData);
    }

    void NewTurn()
    {
        StopCoroutine(nameof(TurnActionRoutine));
        SwitchTeams();
        StartCoroutine(nameof(TurnActionRoutine));
    }
    
    void CompleteActionByUser()
    {
        currentTurnHandler.CompleteAction();
    }

    void SwitchTeams()
    {
        currentTeamType = turnTeams["rivalTeam"].Data.TeamType;
        (turnTeams["currentTeam"], turnTeams["rivalTeam"]) = (turnTeams["rivalTeam"], turnTeams["currentTeam"]);

        Eventbus.UIEvents.OnTeamSwitch?.Invoke(currentTeamType);
        
        // var temp = currentTeam;
        // currentTeam = rivalTeam;
        // rivalTeam = temp;
    }

    bool GameEnding()
    {
        foreach (var team in turnTeams)
        {
            if (team.Value.Data.Towers.Count < 2 || team.Value.Data.Towers.All(t => t.Data.Health == 0))
            {
                Eventbus.NetworkTriggerEvents.OnGameEnds?.Invoke(team.Value.Data.TeamType);
                print("game ends");
                return true;
            }
        }
        return false;
    }
    
    private void OnDisable()
    {
        Eventbus.NetworkRequestEvents.OnCompleteActionRequest -= CompleteActionByUser;
        Eventbus.NetworkRequestEvents.OnNewTurnRequest -= NewTurn;
        Eventbus.NetworkEvents.OnAllClientsSet -= FirstTurn;
    }
}