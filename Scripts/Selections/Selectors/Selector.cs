using System.Collections.Generic;
using System.Linq;
using _Core.Turn.Selectors;
using Actor;
using Enums;
using Enums.Selections;
using GameUI;
using Network;
using Teams;
using Towers;
using UnityEngine;


public abstract class Selector: IBlockable //Selector<T> where T : ISelectionColorSetter, new()
{
    protected SelectionGroup CurrentGroup;
    public SelectionData Data { get; private set; }
    protected Blocker Blocker = new();


    protected Dictionary<TeamState, Team> _teamsByTurn = new();
    
    protected abstract void SubscribeAndSetup(); 
    protected abstract void Unregister();
    public abstract void RestartWithNewTowers();
    protected abstract void GetActor(params object[] args);
    protected abstract void HandleUI();
    public abstract List<uint> SendAllTowers();
    //protected abstract void DeselectAll();
    protected abstract bool SelectedTwice(uint selectedTower);

    public void Subscribe()
    {
        //SelectedTowers.Clear(); //TODO: DONT!
        NetworkEventbus.InputEvents.OnObjectClicked += GetActor;
        SubscribeAndSetup();
    }

    public void SetTeamsAndBlock(Dictionary<TeamState, Team> teams)
    {
        _teamsByTurn = teams;
        Block();
        
        SelectionEvents.OnSelectionReady?.Invoke(this);
    }

    protected void Block()
    {
        AllTowers.EnableClickability();
        
        Blocker.BlockType = CurrentGroup.BlockType;
        Blocker.BlockSelection(_teamsByTurn, CurrentGroup.BlockedTeam);
    }

    public Team GetSelectionTeam(int i)
    {
        var teamState = Data.Groups[i].SelectionTeam;
       return _teamsByTurn[teamState];
    }

    public void SetData(SelectionData data)
    {
        Data = data;
        InitialSetup();
    }

    public abstract void InitialSetup();
    protected void HandleSelection(bool select, uint newSelection)
    {
        if (select)
            Select(newSelection);
        else
            Deselect(newSelection);

        HandleUI();
    }

    private void Select(uint newSelection)
    {
        CurrentGroup.SelectedActors.Add(newSelection);
        var actor = ActorDB.Registry[newSelection];//AllTowers.GetData(newSelection);
        
        SetSelectionColor(actor);

        SelectionEvents.OnSelection?.Invoke(actor.ID); //todo: name

    }
    private void Deselect(uint newSelection)
    {
        CurrentGroup.SelectedActors.Remove(newSelection);
        var actor = ActorDB.Registry[newSelection];//AllTowers.GetData(newSelection);

        foreach (var tower in actor.Towers)
        {
            tower.VisualData.ColorHandler.ToOriginalSelectionColor();
        }
       
        
        SelectionEvents.OnDeselect?.Invoke(actor.ID);
    }

    public void DeselectSelected()
    {
        foreach (var group in Data.Groups)
        {
            group.ResetSelectedTowers();
        }
        
        CurrentGroup = Data.Groups[0]; //is it necessary here?
        Block();
        DeselectCall();
        SelectionEvents.OnDeselectAll?.Invoke();
    }

    public void DeselectAll() //temporary
    {
        foreach (var group in Data.Groups)
        {
            group.ResetAllTowers();
        }
        
        
        CurrentGroup = Data.Groups[0]; //is it necessary here?
        Block();
        DeselectCall();
    }
    

    public void ClearTowers()
    {
        foreach (var group in Data.Groups)
        {
            group.ClearTowers();
        }
    }

    protected virtual void DeselectCall() { }
    
    private void SetSelectionColor(ActorData actor)
    {
        foreach (var tower in actor.Towers)
        {
            tower.VisualData.ColorHandler.SetColorByGivenState(CurrentGroup.selectionColorState);
        }
    }
    protected void HighlightApply(bool enable)
    {
        UIEventbus.OnApplyPossibility?.Invoke(enable);
    }

    public virtual void ResetMaxSelection(){}
    public virtual void SetMaxTowers(int amount) {}
    public virtual void IncreaseMaxTowers() {}
    public virtual void ContinueTowers(List<uint> towers) {}//önceki state'ten kalan varsa takip edebilelim diye
    
    public void Unsubscribe()
    {
        NetworkEventbus.InputEvents.OnObjectClicked -= GetActor;
        // AllTowers.ResetTowerSelectionColors(); //todo: test, dont
        AllTowers.EnableClickability(); //todo: eğer eliminated ise
        Unregister();
    }
}