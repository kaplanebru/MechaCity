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
    Dictionary<string, Team> turnTeams;
    [SerializeField] private TeamsHandler teamsHandler;
    public Team currentTEAM; //DEBUG
    
    private BaseTurnHandler currentTurnHandler;
    
    private void Start()
    {
        SetTurnTeams();
        turnHandlers = GetComponentsInChildren<BaseTurnHandler>(true).ToArray();
        DisableAllTurnHandlers();
        InitializeTeams();

       
        Eventbus.NetworkEvents.OnAllClientsSet += FirstTurn;
        
        Eventbus.NetworkRequestEvents.OnCompleteActionRequest += CompleteActionByUser;
        Eventbus.NetworkRequestEvents.OnNewTurnRequest += NewTurn;
        Eventbus.NetworkRequestEvents.TeamSwitchRequest += SwitchTeams;
        
        Eventbus.TurnEvents.OnInitialize?.Invoke();
       
       
      
    }

  

    void SetTurnTeams()
    {
        turnTeams = new Dictionary<string, Team>()
        {
            {"currentTeam", teamsHandler.teams[0]},
            {"rivalTeam", teamsHandler.teams[1]},
        };
        
        currentTEAM = turnTeams["currentTeam"]; //DEBUG
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
        foreach (var team in turnTeams)
        {
            team.Value.Initialize();
        }

        SetFirstMatches();
    }
    
    public void FirstTurn(Team[] teams)
    {
        StartCoroutine(nameof(TurnActionRoutine));
        //Burda da currentTeam Network variable kullanılmalı
    }
    
    IEnumerator TurnActionRoutine()
    {
        Eventbus.TurnEvents.OnTurnStarted?.Invoke();
        print(currentTEAM.name);
        
        for (var i = 0; i < turnHandlers.Length; i++)
        {
            currentTurnHandler = turnHandlers[i];
            currentTurnHandler.enabled = true;
            currentTurnHandler.SetTeams(turnTeams);

            GetIncomingData(i);
            currentTurnHandler.Setup();

            yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed);
        }

        Eventbus.TurnEvents.OnTurnEnded?.Invoke();
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
        
        Eventbus.NetworkTriggerEvents.OnTeamSwitchSetup?.Invoke(turnTeams["rivalTeam"].Data.TeamType); //new team
        
        StartCoroutine(nameof(TurnActionRoutine));
    }
    
    void CompleteActionByUser()
    {
        //print("completed: " + currentTurnHandler.name);
        currentTurnHandler.CompleteAction();
    }

    void SwitchTeams(TeamType newTeamType)
    {
        (turnTeams["currentTeam"], turnTeams["rivalTeam"]) = (turnTeams["rivalTeam"], turnTeams["currentTeam"]);
        currentTEAM = turnTeams["currentTeam"]; //DEBUG

        // var temp = currentTeam;
        // currentTeam = rivalTeam;
        // rivalTeam = temp;
    }

    void SetFirstMatches() //Temporary
    {
        turnTeams["currentTeam"].LinkFirstMatches(turnTeams["rivalTeam"]);
        turnTeams["rivalTeam"].LinkFirstMatches(turnTeams["currentTeam"]);
    }

    private void OnDisable()
    {
        Eventbus.NetworkRequestEvents.OnCompleteActionRequest -= CompleteActionByUser;
        Eventbus.NetworkRequestEvents.OnNewTurnRequest -= NewTurn;
        Eventbus.NetworkRequestEvents.TeamSwitchRequest -= SwitchTeams;
        
        //Eventbus.NetworkRequestEvents.OnPlayerSpawned -= StartTurn; //temp
        Eventbus.NetworkEvents.OnAllClientsSet -= FirstTurn;

        
    }
}