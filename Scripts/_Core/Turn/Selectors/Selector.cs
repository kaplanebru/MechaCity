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
    public List<int> Towers = new();
    public int SelectionTowerAmount = 2;
    public int MinTowersInGroup = 2;
    private T selectionColorSetter = new T();
    public void Subscribe()
    {
        //Towers.Clear(); //TODO: DONT!
        NetworkEventbus.InputEvents.OnObjectClicked += GetTower;
    }
    

    public void StartTowers(List<int> towers)
    {
        //SelectionTowerAmount = MinTowersInGroup; //temp
        Towers = towers;
    }
    
    public void SetMaxTowers(int amount)
    {
        SelectionTowerAmount = amount;
    }

    private void GetTower(params object[] args)
    {
        int towerId = (int) args[0];

        if (SelectedTwice(towerId)) return;

        if (Towers.Count == SelectionTowerAmount)
            ResetSelectionGroup();

        HandleSelection(true, towerId);
    }

    void HandleSelection(bool select, int newSelection)
    {
        if (select)
            Select(newSelection);
        else
            Deselect(newSelection);

        ShowCompleteButton(Towers.Count == SelectionTowerAmount);
    }
    
    private void Select(int newSelection)
    {
        Towers.Add(newSelection);
        selectionColorSetter.SetColor(newSelection);
    }

    void Deselect(int newSelection)
    {
        Towers.Remove(newSelection);
        AllTowers.GetData(newSelection).ColorHandler.ToOriginalColor();
    }

    void ShowCompleteButton(bool enable)
    {
        UIEventbus.OnButtonCall?.Invoke(enable);
    }

    void ResetSelectionGroup()
    {
        for (int i = 0; i < SelectionTowerAmount; i++)
        {
            HandleSelection(false, Towers[0]);
        }
    }

    bool SelectedTwice(int selectedTower)
    {
        if (Towers.Contains(selectedTower))
        {
            HandleSelection(false, selectedTower);
            return true;
        }

        return false;
    }

    public void ResetSelector()
    {
        SelectionTowerAmount = MinTowersInGroup;
    }

    public void Unsubscribe()
    {
        NetworkEventbus.InputEvents.OnObjectClicked -= GetTower;
        // AllTowers.ResetTowerSelectionColors(); //todo: test, dont
        AllTowers.EnableClickability(); //todo: eğer eliminated ise
    }
}