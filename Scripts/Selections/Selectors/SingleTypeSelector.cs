using System.Collections.Generic;
using _Core.Turn.Selectors;
using Enums;
using Teams;

public class SingleTypeSelector : Selector, IBlockable
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
        DeselectAll();
        //turn bitiminde resetleniyor!!
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

    // protected override void DeselectAll()
    // {
    //     CurrentGroup.ResetTowers();
    // }

    protected override void HandleUI()
    {
        HighlightApply(CurrentGroup.SelectedTowers.Count == CurrentGroup.MaxTowers);
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
