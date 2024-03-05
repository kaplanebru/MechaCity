using Teams;
using Towers;
using UnityEngine;

public class SelectionBlocker : BaseSelector, ISelectionBlocker<TeamData, TeamData>
{
    public TeamData RivalTeam { get;  }
    public SelectionBlocker(TeamData rivalTeam)
    {
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