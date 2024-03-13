using Enums;
using Teams;
using Towers;
using Turn;
using UnityEngine;

public class SelectionStateSelector : BaseSelector, ISelectionBlocker<TeamData>
{
    public TeamData TeamToBlock { get;  }

    public SelectionStateSelector(TeamData rivalTeam) 
    {
        TeamToBlock = rivalTeam;
    }

    public SelectionStateSelector() {}

    public void EliminateNonSelectables()
    {
        TeamToBlock.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).DisableSelection());
    }

    public void EliminateSpecificNonSelectables<TTTeam>(TTTeam teamToBlock) where TTTeam : TeamData
    {
        teamToBlock.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).DisableSelection());
        Debug.Log(teamToBlock.TeamType);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        AllTowers.EnableClickability();
    }

  
}