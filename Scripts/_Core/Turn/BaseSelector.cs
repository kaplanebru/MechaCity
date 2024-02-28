using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using GameUI;
using Network;
using Teams;
using Towers;
using UnityEngine;

// public interface ISelectableGroup<out TSelectionType> where TSelectionType : 
// {
//     public TSelectionType selectionType { get; }
// }

public class PlayerSelector : BaseSelector
{
    public override SelectionType selectionType { get; } = SelectionType.PlayerOnly;

    public PlayerSelector(int maxTowersInGroup) 
    {
        
    }
    
    protected override void SetSelectableTowers()
    {
        Teams[TeamState.RivalTeam].Data.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).clickHandler.DisableSelection());
    }

   
}

public class RivalSelector : BaseSelector
{
    public override SelectionType selectionType { get; } = SelectionType.RivalOnly;
   

    public RivalSelector(int maxTowersInGroup)
    {
        
    }
    
    protected override void SetSelectableTowers()
    {
        Teams[TeamState.CurrentTeam].Data.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).clickHandler.DisableSelection());
    }

   
}

public class IndifferentSelector : BaseSelector, ISelectable<bool>
{
    public override SelectionType selectionType { get; } = SelectionType.All;
    
    public IndifferentSelector(int maxTowersInGroup) 
    {
        
    }
    protected override void SetSelectableTowers() {}

    public bool ForBp { get; }
}

public interface ISelectable<out TType> 
{
    public TType ForBp { get; }
}

public abstract class BaseSelector 
{
    public abstract SelectionType selectionType { get; }
    public List<int> Towers = new();
    protected Dictionary<TeamState, Team> Teams;
    private Material selectionMat;
    private Material defaultMat;

    
    private int _maxTowersInGroup;
    private SelectionType _selectionType;
    
    public void Subscribe()
    {
        Towers.Clear();
        NetworkEventbus.InputEvents.OnObjectClicked += GetTower;
    }
    
    public void SetTeams(Dictionary<TeamState, Team> teams)
    {
        Teams = teams;
        SetSelectableTowers();
        SetMaterials();
    }

    protected abstract void SetSelectableTowers();

    protected virtual void SetMaterials()
    {
        defaultMat = Teams[TeamState.CurrentTeam].Data.TeamTowerData.DefaultMaterial;
        selectionMat = Teams[TeamState.CurrentTeam].Data.TeamTowerData.SelectedMaterial;
    }
 
    
    private void GetTower(params object[] args)
    {
        int towerId = (int) args[0];

        if (SelectedTwice(towerId)) return;

        if (Towers.Count == _maxTowersInGroup)
            ResetSelectionGroup();

        Select(true, towerId);
    }
    
    void Select(bool select, int newSelection)
    {
        if (select)
        {
            Towers.Add(newSelection);
            AllTowers.GetTower(newSelection).towerParts.SetColor(selectionMat);
        }
        else
        {
            Towers.Remove(newSelection);
            AllTowers.GetTower(newSelection).towerParts.SetColor(defaultMat);
        }

        ShowCompleteButton(Towers.Count == _maxTowersInGroup);
    }
    
    void ShowCompleteButton(bool enable)
    {
        UIEventbus.OnButtonCall?.Invoke(enable);
    }
    
    void ResetSelectionGroup()
    {
        for (int i = 0; i < _maxTowersInGroup; i++)
        {
            Select(false, Towers[0]);
        }
    }
    
    bool SelectedTwice(int selectedTower)
    {
        if (Towers.Contains(selectedTower))
        {
            Select(false, selectedTower);
            return true;
        }

        return false;
    }
    
    public void Unsubscribe()
    {
        NetworkEventbus.InputEvents.OnObjectClicked -= GetTower;
        AllTowers.ResetClickability();
    }

    
}
