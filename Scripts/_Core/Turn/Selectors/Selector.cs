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
    public int MaxTowerAmount = 2;
    public int MinTowersInGroup = 2;
    private T selectionColorSetter = new T();
    public void Subscribe()
    {
        //SelectedTowers.Clear(); //TODO: DONT!
        NetworkEventbus.InputEvents.OnObjectClicked += GetTower;
    }
    

    public void ContinueTowers(List<int> towers) //önceki state'ten kalan varsa takip edebilelim diye
    {
        SelectedTowers = towers;
    }

    public void StartWithNewTowers()
    {
        SelectedTowers.Clear();
    }
    
    public void SetMaxTowers(int amount)
    {
        MaxTowerAmount = amount;
    }

    private void GetTower(params object[] args)
    {
        int towerId = (int) args[0];

        if (SelectedTwice(towerId)) return;

        if (SelectedTowers.Count == MaxTowerAmount)
            ResetSelectionGroup();

        HandleSelection(true, towerId);
    }

    void HandleSelection(bool select, int newSelection)
    {
        if (select)
            Select(newSelection);
        else
            Deselect(newSelection);

        ShowCompleteButton(SelectedTowers.Count == MaxTowerAmount);
    }
    
    private void Select(int newSelection)
    {
        SelectedTowers.Add(newSelection);
        selectionColorSetter.SetColor(newSelection);
    }

    void Deselect(int newSelection)
    {
        SelectedTowers.Remove(newSelection);
        AllTowers.GetData(newSelection).ColorHandler.ToOriginalColor();
    }

    void ShowCompleteButton(bool enable)
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

    bool SelectedTwice(int selectedTower)
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
    }
}