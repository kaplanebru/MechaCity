using System.Collections;
using System.Collections.Generic;
using Core;
using Enums;
using Teams;
using Towers;
using Turn;
using UnityEngine;

public class BpSelector : BaseSelector
{
    protected override void Select(int newSelection)
    {
        Towers.Add(newSelection);
        AllTowers.GetTower(newSelection).ToBlueprintColor();
    }
}


public class BpRestrictedSelector : BpSelector, ISelectionBlocker<TeamData>
{
    public TeamData TeamToBlock { get; set; }
    public BpRestrictedSelector(TeamData rivalTeam) 
    {
        TeamToBlock = rivalTeam;
    }

    public BpRestrictedSelector() {}
    public void EliminateNonSelectables()
    {
        TeamToBlock.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).DisableSelection());
    }

    public void EliminateSpecificNonSelectables<TTTeam>(TTTeam teamToBlock) where TTTeam : TeamData
    {
        teamToBlock.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).DisableSelection());
    }
    
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        AllTowers.EnableClickability(); //todo: temp maybe
    }

}

