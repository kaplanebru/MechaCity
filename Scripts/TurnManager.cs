using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    BaseTurnHandler[] turnHandlers;
    Dictionary<string, Team> teams;

    //aşağıdakileri asset holdera koy
    [SerializeField]private Team currentTeam;
    [SerializeField]private Team rivalTeam;
    
    private void Start()
    {
        SetTeams();
        turnHandlers = GetComponentsInChildren<BaseTurnHandler>(true).ToArray();
        DisableAllTurnHandlers();
        InitializeTeams();
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
    
    IEnumerator TurnActionRoutine()
    {
        for (var i = 0; i < turnHandlers.Length; i++)
        {
            BaseTurnHandler currentTurnHandler = turnHandlers[i];
            currentTurnHandler.enabled = true;
            
            currentTurnHandler.SetTeams(teams);

            GetIncomingData(i, currentTurnHandler);
            currentTurnHandler.Setup();

            yield return new WaitUntil(() => currentTurnHandler.turnAction == TurnAction.Completed);
        }
        
        Eventbus.TurnEvents.OnTurnCompleted?.Invoke();
        NewTurn();
    }
    
    void GetIncomingData(int turnIndex, BaseTurnHandler currentTurnHandler)
    {
        if (turnIndex <= 0) return;
        
        var transferData = ((ITurnActionHandler<BaseTurnData>)turnHandlers[turnIndex - 1]).Data;
        currentTurnHandler.ProcessIncomingData(transferData);
    }

    void NewTurn()
    {
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

  
    
}
    