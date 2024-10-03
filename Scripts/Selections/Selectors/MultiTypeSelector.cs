using System;
using System.Collections.Generic;
using _Core.Turn.Selectors;
using Enums;
using Enums.Selections;
using Teams;
using Towers;
using UnityEngine;

public class MultiTypeSelector : Selector, IBlockable
{
    public override void InitialSetup() {}
    
    private bool isFull = false;

    protected override void SubscribeAndSetup()
    {
        CurrentGroup = Data.Groups[0];
        TeamEvents.OnTeamsSent += SetTeamsAndBlock;
    }

    protected override void Unregister()
    {
        TeamEvents.OnTeamsSent -= SetTeamsAndBlock;
    }

    public override void RestartWithNewTowers()
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

   

    protected override void DeselectCall() => isFull = false;
   

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

    void CheckConstraints()
    {
        
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