using System.Collections;
using System.Collections.Generic;
using _Core.Turn.Selectors;
using Towers;
using UnityEngine;

public class SingleTypeSelector : Selector<StandardSelectionColor>
{
   
    private int _maxTowerConstant;
    protected override void Register()
    {
        CurrentGroup = Data.Groups[0];
        _maxTowerConstant = CurrentGroup.MaxTowers;
    }

    protected override void Unregister() {}

    public override void StartWithNewTowers()
    {
        CurrentGroup.SelectedTowers.Clear();
    }
    
    protected override void GetTower(params object[] args)
    {
        int towerId = (int) args[0];

        if (SelectedTwice(towerId)) return;

        if (CurrentGroup.SelectedTowers.Count == CurrentGroup.MaxTowers)
            DeselectAll();

        HandleSelection(true, towerId);
    }
    
    protected override bool SelectedTwice(int selectedTower)
    {
        if (CurrentGroup.SelectedTowers.Contains(selectedTower))
        {
            HandleSelection(false, selectedTower);
            return true;
        }
        return false;
    }
    
    public override void ContinueTowers(List<int> towers) //önceki state'ten kalan varsa takip edebilelim diye
    {
        CurrentGroup.SelectedTowers = towers;
    }

    protected override void DeselectAll()
    {
        CurrentGroup.ResetTowers();
    }

    protected override void HandleUI()
    {
        ShowCompleteButton(CurrentGroup.SelectedTowers.Count == CurrentGroup.MaxTowers);
    }
    
    public override void ResetMaxSelection()
    {
        CurrentGroup.MaxTowers = _maxTowerConstant;
    }
    
    public override void SetMaxTowers(int amount)
    {
        CurrentGroup.MaxTowers = amount;
    }
    
    public override void IncreaseMaxTowers()
    {
        CurrentGroup.MaxTowers++;
    }
    
    public override List<int> SendAllTowers()
    {
        return CurrentGroup.SelectedTowers;
    }
    
}
