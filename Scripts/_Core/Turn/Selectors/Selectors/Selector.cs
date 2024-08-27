using System;
using System.Collections.Generic;
using _Core.Turn.Selectors;
using Enums;
using GameUI;
using Network;
using Teams;
using Towers;
using Turn;
using UnityEngine;


public abstract class Selector //Selector<T> where T : ISelectionColorSetter, new()
{
    protected SelectionGroup CurrentGroup;
    protected SelectionData Data;
    protected Blocker Blocker = new();


    //protected T selectionColorSetter = new T();
    protected Dictionary<TeamState, Team> _teams = new();
    
    public void Subscribe()
    {
        //SelectedTowers.Clear(); //TODO: DONT!
        NetworkEventbus.InputEvents.OnObjectClicked += GetTower;
        Register();
    }

    public void SetData(SelectionData data)
    {
        Data = data;
    }

    protected abstract void Register(); 
    protected abstract void Unregister();
    public abstract void StartWithNewTowers();
    protected abstract void GetTower(params object[] args);
    protected abstract void HandleUI();
    public abstract List<int> SendAllTowers();
    protected abstract void DeselectAll();
    protected abstract bool SelectedTwice(int selectedTower);

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
        SetSelectionColor(newSelection);
       
    }
    private void Deselect(int newSelection)
    {
        CurrentGroup.SelectedTowers.Remove(newSelection);
        AllTowers.GetData(newSelection).ColorHandler.ToOriginalColor();
    }
    
    private void SetSelectionColor(int newSelection)
    {
        AllTowers.GetData(newSelection).ColorHandler.SetColorByType(CurrentGroup.SelectionColorType);
        //selectionColorSetter.SetColor(newSelection);
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