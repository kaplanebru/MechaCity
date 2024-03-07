using Teams;
using Towers;
using Turn;
using UnityEngine;

public class SelectionBlocker : BaseSelector<SelectionTransferData>, ISelectionBlocker<TeamData, TeamData>
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


    public override SelectionTransferData turnData { get; set; } = new SelectionTransferData();

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        AllTowers.EnableClickability();
    }
}