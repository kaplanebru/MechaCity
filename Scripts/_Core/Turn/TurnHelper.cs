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
    public Dictionary<TeamState, Team> TeamsByTurn;
    
    public TeamType CurrentTeamType = TeamType.Team1;

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
        //return _stateHolder.States[nextStateId].StateType;
    }

    public void SwitchTeams()
    {
        CurrentTeamType = TeamsByTurn[TeamState.RivalTeam].Data.TeamType;
        
        (TeamsByTurn[TeamState.CurrentTeam], TeamsByTurn[TeamState.RivalTeam]) =
            (TeamsByTurn[TeamState.RivalTeam], TeamsByTurn[TeamState.CurrentTeam]);

        UIEventbus.OnTeamSwitch?.Invoke(CurrentTeamType);
    }

    public void ManageInput()
    {
        if (!MultiplayerSetter.IsMultiplayerOn) return;
        TeamsByTurn[TeamState.CurrentTeam].Data.Player.EnableInput(true);
        TeamsByTurn[TeamState.RivalTeam].Data.Player.EnableInput(false);
    }
    
    public bool GameEnding()
    {
        foreach (var team in TeamsByTurn)
        {
            
            if (team.Value.Data.Actors.Count < 2) //|| team.Value.Data.Towers.All(t => ActorHolder.GetHealthByActor(t.UniqID) == 0)) //Turn sonunda Health'in 0 olarak kaldığı bir case yok, 0 olan dönüşüyor
            {
                if (team.Value.Data.Actors[0].TowerAmount > 1) return false;
                
                NetworkEventbus.UserEvents.OnGameEnds?.Invoke(team.Value.Data.TeamType);
                Debug.Log("game ends");
                return true;
            }
        }

        return false;
    }

}
