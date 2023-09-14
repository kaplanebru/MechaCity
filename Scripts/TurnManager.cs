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
    [SerializeField] private TeamsHandler teamsHandler;


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

            GetIncomingData(i);
            currentTurnHandler.Setup();

            yield return
                new WaitUntil(() =>
                    currentTurnHandler.turnAction ==
                    TurnAction.Completed); //TODO: bool Network variable yapılabilir. Tıklayınca complete oluyor.
        }

        Eventbus.TurnEvents.OnTurnCompleted?.Invoke();
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

    void SwitchTeams()
    {
        (teams["currentTeam"], teams["rivalTeam"]) = (teams["rivalTeam"], teams["currentTeam"]);

        //print("new: " + teams["currentTeam"].name);

        // var temp = currentTeam;
        // currentTeam = rivalTeam;
        // rivalTeam = temp;
    }

    void SetFirstMatches() //Temporary
    {
        teams["currentTeam"].LinkFirstMatches(teams["rivalTeam"]);
        teams["rivalTeam"].LinkFirstMatches(teams["currentTeam"]);
    }

    private void OnDisable()
    {
        Eventbus.NetworkEvents.OnTurnHandleTypeChanged -= CompleteCurrentAction;
    }
}