using System;
using System.Collections.Generic;
using Enums;
using GameUI;
using Network;
using Towers;
using Turn;
using UnityEngine;

public abstract class BaseSelector<TType> where TType : BaseTurnTransferData
{
    public abstract TType turnData { get; set; }
    public List<int> Towers = new();
    public int _maxTowersInGroup = 2;

    
    
    public void Subscribe()
    {
        //Towers.Clear(); //TODO: DONT!
        NetworkEventbus.InputEvents.OnObjectClicked += GetTower;
        ShowCompleteButton(false);
    }

    public void GetTowers(List<int> towers)
    {
        Towers = towers;
    }
    
    private void GetTower(params object[] args)
    {
        int towerId = (int) args[0];

        if (SelectedTwice(towerId)) return;

        if (Towers.Count == _maxTowersInGroup)
            ResetSelectionGroup();

        HandleSelection(true, towerId);
    }
    
    void HandleSelection(bool select, int newSelection)
    {
        if (select)
            Select(newSelection);
        else
            Deselect(newSelection);
        
        ShowCompleteButton(Towers.Count == _maxTowersInGroup);
    }

    protected virtual void Select(int newSelection)
    {
        Towers.Add(newSelection);
        AllTowers.GetTower(newSelection).ToSelectionColor();
        //AllTowers.GetTower(newSelection).ToGivenColor(selectionMat); //to pp color: zaten 2 tarafın bp colorı farklı nüansta olur
    }

    void Deselect(int newSelection)
    {
        Towers.Remove(newSelection);
        AllTowers.GetTower(newSelection).ToOriginalColor();
    }
    
    void ShowCompleteButton(bool enable) //virtual yapılabilir
    {
        UIEventbus.OnButtonCall?.Invoke(enable, turnData.StateType);
    }
    
    void ResetSelectionGroup()
    {
        for (int i = 0; i < _maxTowersInGroup; i++)
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
    
    public virtual void Unsubscribe()
    {
        NetworkEventbus.InputEvents.OnObjectClicked -= GetTower;
    }


}
