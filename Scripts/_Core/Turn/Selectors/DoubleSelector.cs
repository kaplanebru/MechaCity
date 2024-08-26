using System.Collections;
using System.Collections.Generic;
using _Core.Turn.Selectors;
using Enums;
using Network;
using Teams;
using Towers;
using Turn;
using UnityEngine;

public class DoubleSelector : Selector<BpSelectionColor>
{
    private PlayerBlocker playerBlocker = new PlayerBlocker();
    private RivalBlocker rivalBlocker = new RivalBlocker();
    
    public int groupAmount = 2;
    public SelectionGroup[] SelectionGroups;
    private bool isFull = false;
    

    private SelectionGroup _currentSelectionGroup;

    protected override void Register()
    {
        TurnHelper.TurnEvents.OnTeamsSent += GetTeamsData;
    }

    protected override void Unregister()
    {
        TurnHelper.TurnEvents.OnTeamsSent -= GetTeamsData;
    }
    
    public override void StartWithNewTowers()
    {
        TurnHelper.TurnEvents.OnTeamsRequest?.Invoke();
        SelectionGroups = new SelectionGroup[groupAmount];
        for (int i = 0; i < groupAmount; i++)
        {
            SelectionGroups[i] = new SelectionGroup();
            SelectionGroups[i].Index = i;
            
        }
        
        SelectionGroups[0].BlockType = TeamState.RivalTeam;
        SelectionGroups[1].BlockType = TeamState.CurrentTeam;

        Setup();
        _currentSelectionGroup = SelectionGroups[0];
    }

    public void GetTeamsData(Dictionary<TeamState, Team> teams) //sürekli değiştiği için, burda almakta fayda var
    {
        _teams = teams;
    }

    void Setup()
    {
        for (int i = 0; i < groupAmount; i++)
        {
            SelectionGroups[i].MaxTowers = 1; 
        }
    }
    
    protected override void GetTower(params object[] args)
    {
        int towerId = (int) args[0];
        
        if (isFull)
        {
            ResetSelectionGroups(); //hepsi deselect olur
            isFull = false;
            ShowCompleteButton(false);
        }
        
        if(SelectedTwice(towerId)) return; //sadece seçili olan deselect olur. TODO: tıklanamaz olduğu için sıkıntı
        
        HandleSelection(true, towerId);
        
        if (_currentSelectionGroup.SelectedTowers.Count == _currentSelectionGroup.MaxTowers)
        {
            ShiftGroup();
        }
    }

    void ShiftGroup()
    {
        int nextGroupIndex = _currentSelectionGroup.Index + 1; // % groupAmount - 1;

        if (nextGroupIndex == groupAmount)
        {
            FullSituation();
            nextGroupIndex = 0;
        }
            
        _currentSelectionGroup = SelectionGroups[nextGroupIndex];
        Block();
    }
    
    void Block()
    {
        AllTowers.EnableClickability();
        
        if (_currentSelectionGroup.BlockType == TeamState.CurrentTeam)
            playerBlocker.BlockSelection(_teams);
        else
            rivalBlocker.BlockSelection(_teams);
    }

    void FullSituation()
    {
        ShowCompleteButton(true);
        isFull = true;
    }
  
    
    protected override void Select(int newSelection)
    {
        _currentSelectionGroup.SelectedTowers.Add(newSelection);
        selectionColorSetter.SetColor(newSelection); //TODO: blocked team'e göre değişir
    }

    protected override void Deselect(int newSelection)
    {
        _currentSelectionGroup.SelectedTowers.Remove(newSelection);
        AllTowers.GetData(newSelection).ColorHandler.ToOriginalColor();
    }

    void ResetSelectionGroups()
    {
        foreach (var group in SelectionGroups)
        {
            group.ResetTowers();
        }
    }
}