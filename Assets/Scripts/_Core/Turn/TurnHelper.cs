using System;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using GameUI;
using Health;
using Network;
using Teams;
using Testing;
using Turn;
using UnityEngine;

public class TurnHelper
{
    public Dictionary<TeamStatus, Team> TeamsByTurn;

    public void Subscribe()
    {
        TeamEvents.OnBothTeamsRequest += SendTeams;
    }

    public void Unsubscribe()
    {
        TeamEvents.OnBothTeamsRequest -= SendTeams;
    }

    void SendTeams()
    {
        TeamEvents.OnTeamsSent?.Invoke(TeamsByTurn);
    }

    public void GetPreviousStateData(BaseTurnState previousState, BaseTurnState currentState)
    {
        if (previousState == null) return;

        var previousTransferData = ((ITransferDataHolder<BaseTurnTransferData>) previousState).TransferData;
        currentState.ProcessPreviousStateTransferData(previousTransferData);
    }

    public int GetNextStateId(int currentStateId)
    {
        var nextStateId = (currentStateId + 1) % (TurnStateHolder.StateCount - 1);
        return nextStateId;
    }

    public void SwitchTeams()
    {
        (TeamsByTurn[TeamStatus.ActiveTeam], TeamsByTurn[TeamStatus.PassiveTeam]) =
            (TeamsByTurn[TeamStatus.PassiveTeam], TeamsByTurn[TeamStatus.ActiveTeam]);

    }
    //TeamsByTurn[TeamStatus.ActiveTeam].Data.Player.EnableInput(true);
    public bool GameEnding()
    {
        foreach (var team in TeamsByTurn)
        {
            if(team.Value.Data.Actors.Count == 1)
                if (team.Value.Data.Actors[0].TowerAmount > 1) return false;//todo: check
            
            if (team.Value.Data.Actors.Count < 2)
            {
                NetworkEventbus.UserEvents.OnGameEnds?.Invoke(team.Value.Data.TeamType);
                Debug.Log("game ends");
                return true;
            }
        }

        return false;
    }
}