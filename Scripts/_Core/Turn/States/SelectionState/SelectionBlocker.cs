using Enums;
using Teams;
using Towers;
using Turn;
using UnityEngine;

public class SelectionBlocker : BaseSelector, ISelectionBlocker<TeamData, TeamData>
{
    public TeamData RivalTeam { get;  }
    public sealed override TurnStateType StateType { get; set; }
    public SelectionBlocker(TurnStateType stateType, TeamData rivalTeam) : base(stateType)
    {
        StateType = stateType;
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