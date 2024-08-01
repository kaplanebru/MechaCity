using System.Collections;
using System.Collections.Generic;
using _Core.Turn.Selectors;
using Enums;
using Network;
using Teams;
using Towers;
using UnityEngine;

public class DoubleSelector : Selector<BpSelectionColor>
{
    public int groupAmount = 2;
    public SelectionGroup[] SelectionGroups;
    public int[] MaxTowerAmounts; //eventle gelir
    private Dictionary<TeamState, Team> _teams = new();


    private SelectionGroup _currentSelectionGroup;
    
    public override void StartWithNewTowers()
    {
        SelectionGroups = new SelectionGroup[groupAmount];
        // foreach (var group in SelectionGroup)
        // {
        //    group.SelectedTowers.Clear();
        // }
        
        Setup();
        _currentSelectionGroup = SelectionGroups[0];
    }

    void GetTeamsData(Dictionary<TeamState, Team> teams) //sürekli değiştiği için, burda almakta fayda var
    {
        _teams = teams;
    }

    void Setup()
    {
        for (int i = 0; i < groupAmount; i++)
        {
            SelectionGroups[i].Index = i;
            SelectionGroups[i].MaxTowers = MaxTowerAmounts[i];
        }
    }
    
    protected override void GetTower(params object[] args)
    {
        int towerId = (int) args[0];
        //if (!CheckType(towerId)) return;
        
        if (SelectedTwice(towerId)) return; //buna da bakmak lazım

        if (_currentSelectionGroup.SelectedTowers.Count == _currentSelectionGroup.MaxTowers)
        {
            int nextIndex = _currentSelectionGroup.Index + 1;
            if (nextIndex == groupAmount - 1)
            {
                ResetSelectionGroups();
                return;
            }

            ToNextGroup(nextIndex);
        }
        
        HandleSelection(true, towerId);
    }

    void ToNextGroup(int nextIndex)
    {
        _currentSelectionGroup = SelectionGroups[nextIndex];
        Block();
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
    void Block()
    {
        TeamData teamToBlock = _teams[_currentSelectionGroup.BlockType].Data;
        AllTowers.EnableClickability();
        foreach (var tower in teamToBlock.Towers)
        {
            tower.clickHandler.DisableSelection();
        }
    }
    
    void ResetSelectionGroups()
    {
        
    }


    // bool CheckType(int id)
    // {
    //     return AllTowers.GetData(id).TeamType == _currentSelectionGroup.Type;
    // }
}

public class SelectionGroup
{
    public int Index;
    public List<int> SelectedTowers = new();
    public int MaxTowers = 1;
    public TeamState Type;
    public TeamState BlockType;


}