using System.Collections.Generic;
using System.Linq;
using _Core.Turn.Selectors;
using Enums;
using Enums.Selections;
using GameUI;
using Network;
using Teams;
using Towers;
using UnityEngine;


public abstract class Selector //Selector<T> where T : ISelectionColorSetter, new()
{
    protected SelectionGroup CurrentGroup;
    public SelectionData Data { get; private set; }
    protected Blocker Blocker = new();


    protected Dictionary<TeamState, Team> _teamsByTurn = new();
    
    protected abstract void Register(); 
    protected abstract void Unregister();
    public abstract void StartWithNewTowers();
    protected abstract void GetTower(params object[] args);
    protected abstract void HandleUI();
    public abstract List<int> SendAllTowers();
    protected abstract void DeselectAll();
    protected abstract bool SelectedTwice(int selectedTower);

    public void Subscribe()
    {
        //SelectedTowers.Clear(); //TODO: DONT!
        NetworkEventbus.InputEvents.OnObjectClicked += GetTower;
        Register();
    }

    protected void SetTeams(Dictionary<TeamState, Team> teams)
    {
        _teamsByTurn = teams;
        SelectionEvents.OnSelectionReady?.Invoke(this);
    }

    public Team GetSelectionTeam(int i)
    {
        var teamState = Data.Groups[i].SelectionTeam;
       return _teamsByTurn[teamState];
    }

    public void SetData(SelectionData data)
    {
        Data = data;
    }
    protected void HandleSelection(bool select, int newSelection)
    {
        if (select)
            Select(newSelection);
        else
            Deselect(newSelection);

        HandleUI();
    }

    private void Select(int newSelection)
    {
        CurrentGroup.SelectedTowers.Add(newSelection);
        var tower = AllTowers.GetData(newSelection);
        SetSelectionColor(tower);

        SelectionEvents.OnSelection?.Invoke(tower.UniqID.ToString(), tower.UniqID); //todo: name

    }
    private void Deselect(int newSelection)
    {
        CurrentGroup.SelectedTowers.Remove(newSelection);
        var tower = AllTowers.GetData(newSelection);
        tower.ColorHandler.ToOriginalColor();
        
        SelectionEvents.OnDeselect?.Invoke(tower.UniqID);
    }

   
    
    private void SetSelectionColor(TowerData tower)
    {
        tower.ColorHandler.SetColorByColorType(CurrentGroup.SelectionColorType);
    }
    protected void ShowCompleteButton(bool enable)
    {
        UIEventbus.OnButtonCall?.Invoke(enable);
    }

    public virtual void ResetMaxSelection(){}
    public virtual void SetMaxTowers(int amount) {}
    public virtual void IncreaseMaxTowers() {}
    public virtual void ContinueTowers(List<int> towers) {}//önceki state'ten kalan varsa takip edebilelim diye
    
    public void Unsubscribe()
    {
        NetworkEventbus.InputEvents.OnObjectClicked -= GetTower;
        // AllTowers.ResetTowerSelectionColors(); //todo: test, dont
        AllTowers.EnableClickability(); //todo: eğer eliminated ise
        Unregister();
    }
}