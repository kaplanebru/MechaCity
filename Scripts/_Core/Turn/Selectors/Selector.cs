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


public class Selector<T> where T : ISelectionColorSetter, new()
{
    public List<int> SelectedTowers = new();
    public int MaxTowerAmount = 2; //Array yollarız
    public int MinTowersInGroup = 2; //Todo selection artınca tekrar resetlemek için kullanılıyor
    protected T selectionColorSetter = new T();
    protected Dictionary<TeamState, Team> _teams = new();

    public void Subscribe()
    {
        //SelectedTowers.Clear(); //TODO: DONT!
        NetworkEventbus.InputEvents.OnObjectClicked += GetTower;
        Register();
    }

     protected virtual void Register(){}
     protected virtual void Unregister(){}


    public void ContinueTowers(List<int> towers) //önceki state'ten kalan varsa takip edebilelim diye
    {
        SelectedTowers = towers;
    }

    public virtual void StartWithNewTowers()
    {
        SelectedTowers.Clear();
    }
    
    public void SetMaxTowers(int amount)
    {
        MaxTowerAmount = amount;
    }

    protected virtual void GetTower(params object[] args)
    {
        int towerId = (int) args[0];

        if (SelectedTwice(towerId)) return;

        if (SelectedTowers.Count == MaxTowerAmount)
            ResetSelectionGroup();

        HandleSelection(true, towerId);
    }

    protected void HandleSelection(bool select, int newSelection)
    {
        if (select)
            Select(newSelection);
        else
            Deselect(newSelection);

        ShowCompleteButton(SelectedTowers.Count == MaxTowerAmount);
    }
    
    protected virtual void Select(int newSelection)
    {
        SelectedTowers.Add(newSelection);
        selectionColorSetter.SetColor(newSelection);
    }

    protected virtual void Deselect(int newSelection)
    {
        SelectedTowers.Remove(newSelection);
        AllTowers.GetData(newSelection).ColorHandler.ToOriginalColor();
    }

    protected void ShowCompleteButton(bool enable)
    {
        UIEventbus.OnButtonCall?.Invoke(enable);
    }

    void ResetSelectionGroup()
    {
        for (int i = 0; i < MaxTowerAmount; i++)
        {
            HandleSelection(false, SelectedTowers[0]);
        }
    }

    protected bool SelectedTwice(int selectedTower)
    {
        if (SelectedTowers.Contains(selectedTower))
        {
            HandleSelection(false, selectedTower);
            return true;
        }

        return false;
    }

    public void ResetSelector()
    {
        MaxTowerAmount = MinTowersInGroup;
    }

    public void Unsubscribe()
    {
        NetworkEventbus.InputEvents.OnObjectClicked -= GetTower;
        // AllTowers.ResetTowerSelectionColors(); //todo: test, dont
        AllTowers.EnableClickability(); //todo: eğer eliminated ise
        Unregister();
    }
}