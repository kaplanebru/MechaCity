using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Enums;
using GameUI;
using Network;
using Teams;
using Towers;
using Unity.VisualScripting;
using UnityEngine;


// public class PlayerSelector : BaseSelector
// {
//
//     public PlayerSelector(int maxTowersInGroup) 
//     {
//         
//     }
//     
//     protected override void SetSelectableTowers()
//     {
//         Teams[TeamState.RivalTeam].Data.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).clickHandler.DisableSelection());
//     }
//
//    
// }
//
// public class RivalSelector : BaseSelector
// {
//    
//
//     public RivalSelector(int maxTowersInGroup)
//     {
//         
//     }
//     
//     protected override void SetSelectableTowers()
//     {
//         Teams[TeamState.CurrentTeam].Data.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).clickHandler.DisableSelection()); 
//         //current değil enemy falan yazmalı, en baştan belirli olmalı
//     }
//
//    
// }



// public class Selector : BaseSelector<TeamType>
// {
//     public TeamState State { get; }
//
//     public Selector(int maxTowersInGroup)
//     {
//         
//     }
//
//     protected override void SetSelectableTowers()
//     {
//         Teams[State].Data.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).clickHandler.DisableSelection());
//     }
//     
// }





public interface ISelectionBlocker<out TTeam, out TTTeam> where TTeam : TeamData where TTTeam : TeamData
{
    public TTeam SelectingTeam { get; }
    
    public TTTeam RivalTeam { get;  }
    public void SetSelectableTowers();

    public void SetMaterials();

}
public class Test
{
  
    
   // private SelfSelector test = new SelfSelector(Team1);
    private BaseSelector test2 = new BaseSelector();
}

public class SelfSelector : BaseSelector, ISelectionBlocker<TeamData, TeamData>
{
    public TeamData SelectingTeam { get; }
    public TeamData RivalTeam { get;  }
    public SelfSelector(TeamData selectingTeam, TeamData rivalTeam)
    {
        SelectingTeam = selectingTeam;
        RivalTeam = rivalTeam;
        SetSelectableTowers();
        SetMaterials();
        
        Subscribe();
    }
    public void SetSelectableTowers()
    {
        // var opponent = Initializer.Teams.FirstOrDefault(t => t != SelectingTeam).Data;
        // opponent.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).clickHandler.DisableSelection());
        RivalTeam.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).clickHandler.DisableSelection());
    }

    public void SetMaterials()
    {
        defaultMat = SelectingTeam.TeamTowerData.DefaultMaterial;
        selectionMat = SelectingTeam.TeamTowerData.SelectedMaterial;
    }
}

public  class BaseSelector
{

    public void Print<T>(T input)
    {
        Debug.Log(input);
    }
    

    public List<int> Towers = new();
    protected Dictionary<TeamState, Team> Teams;
    protected Material selectionMat;
    protected Material defaultMat;
    
    private int _maxTowersInGroup = 2;
    
    public void Subscribe()
    {
        Towers.Clear();
        NetworkEventbus.InputEvents.OnObjectClicked += GetTower;
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
