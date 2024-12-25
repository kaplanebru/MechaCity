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

        foreach (var group in Data.Groups)//temp
        {
            group.SelectedActors.Clear(); 
        }
        
        TeamEvents.OnTeamsSent += SetTeamsAndBlock;
    }

    protected override void Unregister()
    {
        TeamEvents.OnTeamsSent -= SetTeamsAndBlock;
    }

    public override void RestartWithNewTowers()
    {
        //DeselectSelected();
        DeselectAll();

        if (CurrentGroup.BlockType != BlockType.None) //bunu sileriz
            TeamEvents.OnBothTeamsRequest?.Invoke();
    }
    
    protected override void GetActor(params object[] args)
    {
        uint actorID = (uint) args[0];
        
        if (isFull)
        {
            DeselectSelected();
            isFull = false;
            HighlightApply(false);
        }

        // if(SelectedTwice(towerId)) return; //sadece seçili olan deselect olur. TODO: tıklanamaz olduğu için sıkıntı

        HandleSelection(true, actorID);

        if (CurrentGroup.SelectedActors.Count == CurrentGroup.MaxTowers)
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

    protected override bool SelectedTwice(uint selectedActor)
    {
        return false;
    }

    public override List<uint> SendAllTowers()
    {
        List<uint> actors = new();
        foreach (var group in Data.Groups)
        {
            actors.AddRange(group.SelectedActors);
        }

        return actors;
    }

}