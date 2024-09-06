using System.Collections.Generic;
using _Core.Turn.Selectors;
using Enums;
using Enums.Selections;
using Teams;
using Towers;
using UnityEngine;

public class MultiTypeSelector : Selector, IBlockable
{
    
    private bool isFull = false;

    protected override void Register()
    {
        TeamEvents.OnTeamsSent += SetTeamsAndBlock;
    }

    protected override void Unregister()
    {
        TeamEvents.OnTeamsSent -= SetTeamsAndBlock;
    }

    public override void StartWithNewTowers()
    {
        DeselectAll();

        if (CurrentGroup.BlockType != BlockType.None) //bunu sileriz
            TeamEvents.OnTeamsRequest?.Invoke();
    }
    
    protected override void GetTower(params object[] args)
    {
        int towerId = (int) args[0];
        
        if (isFull)
        {
            DeselectAll();
            isFull = false;
            HighlightApply(false);
        }

        // if(SelectedTwice(towerId)) return; //sadece seçili olan deselect olur. TODO: tıklanamaz olduğu için sıkıntı

        HandleSelection(true, towerId);

        if (CurrentGroup.SelectedTowers.Count == CurrentGroup.MaxTowers)
        {
            ShiftGroup();
        }
    }

    protected override void HandleUI() { }

    void ShiftGroup()
    {
        int nextGroupIndex = CurrentGroup.Index + 1; 

        if (nextGroupIndex == Data.Groups.Length)
        {
            FullSituation();
            nextGroupIndex = 0;
        }

        CurrentGroup = Data.Groups[nextGroupIndex];
        Block();
    }

    public void SetTeamsAndBlock(Dictionary<TeamState, Team> teamsByTurn)
    {
        SetTeams(teamsByTurn);
        Block();
    }

    void Block()
    {
        AllTowers.EnableClickability();
        
        Blocker.BlockType = CurrentGroup.BlockType;
        Blocker.BlockSelection(_teamsByTurn, CurrentGroup.BlockedTeam);
    }
    
    void FullSituation()
    {
        HighlightApply(true);
        isFull = true;
    }

    protected override bool SelectedTwice(int selectedTower)
    {
        return false;
    }

    public override List<int> SendAllTowers()
    {
        List<int> towers = new();
        foreach (var group in Data.Groups)
        {
            towers.AddRange(group.SelectedTowers);
        }

        return towers;
    }

}