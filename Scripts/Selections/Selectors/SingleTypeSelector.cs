using System.Collections.Generic;
using _Core.Turn.Selectors;
using Enums;
using Teams;
using UnityEngine;

public class SingleTypeSelector : Selector, IBlockable
{
   
    private int _maxTowerConstant;
    
    public override void InitialSetup()
    {
        _maxTowerConstant = Data.Groups[0].MaxTowers;
    }
    protected override void SubscribeAndSetup()
    {
        CurrentGroup = Data.Groups[0];
    }

    protected override void Unregister() {}

    public override void RestartWithNewTowers()
    {
        DeselectAll();
        //turn bitiminde resetleniyor!!
    }
    
 
    protected override void GetTower(params object[] args)
    {
        uint actorID = (uint) args[0];

        if (SelectedTwice(actorID)) return;

        if (CurrentGroup.SelectedActors.Count == CurrentGroup.MaxTowers)
            DeselectAll();

        HandleSelection(true, actorID);
    }

    protected override bool SelectedTwice(uint selectedActor)
    {
        if (CurrentGroup.SelectedActors.Contains(selectedActor))
        {
            HandleSelection(false, selectedActor);
            return true;
        }
        return false;
    }
    
    public override void ContinueTowers(List<uint> actors) //önceki state'ten kalan varsa takip edebilelim diye
    {
        CurrentGroup.SelectedActors = actors;
    }

    protected override void HandleUI()
    {
        HighlightApply(CurrentGroup.SelectedActors.Count == CurrentGroup.MaxTowers);
    }
    
    public override void ResetMaxSelection()
    {
        CurrentGroup.MaxTowers = _maxTowerConstant;
    }
    
    public void ResetByForce()
    {
        _maxTowerConstant = 2;
        ResetMaxSelection();
        
        SelectionEvents.OnSelectionReady?.Invoke(this);
    }
    
    public override void SetMaxTowers(int amount)
    {
        CurrentGroup.MaxTowers = amount;
    }
    
    public override void IncreaseMaxTowers()
    {
        CurrentGroup.MaxTowers++;
    }
    
    public override List<uint> SendAllTowers()
    {
        return CurrentGroup.SelectedActors;
    }

    
}
