using Teams;
using Towers;
using UnityEngine;

public class SelfSelector : BaseSelector, ISelectionBlocker<TeamData, TeamData>
{
    public TeamData SelectingTeam { get; }
    public TeamData RivalTeam { get;  }
    public SelfSelector(TeamData selectingTeam, TeamData rivalTeam)
    {
        SelectingTeam = selectingTeam;
        RivalTeam = rivalTeam;
    }
    
    public void EliminateNonSelectables()
    {
        RivalTeam.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).clickHandler.DisableSelection());
    }
    

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        AllTowers.EnableClickability();
    }
}