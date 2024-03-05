using System.Collections.Generic;
using GameUI;
using Network;
using Towers;
using UnityEngine;

public abstract class BaseSelector
{
    public List<int> Towers = new();
    public int _maxTowersInGroup = 2;
    
    protected Material selectionMat;
    protected Material defaultMat;

   
    public abstract void SetMaterials();
    
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

        Select(true, towerId);
    }
    
    void Select(bool select, int newSelection)
    {
        if (select)
        {
            Towers.Add(newSelection);
            AllTowers.GetTower(newSelection).towerParts.SetColor(selectionMat);
        }
        else
        {
            Towers.Remove(newSelection);
            AllTowers.GetTower(newSelection).towerParts.SetColor(defaultMat);
        }

        ShowCompleteButton(Towers.Count == _maxTowersInGroup);
    }
    
    void ShowCompleteButton(bool enable)
    {
        UIEventbus.OnButtonCall?.Invoke(enable);
    }
    
    void ResetSelectionGroup()
    {
        for (int i = 0; i < _maxTowersInGroup; i++)
        {
            Select(false, Towers[0]);
        }
    }
    
    bool SelectedTwice(int selectedTower)
    {
        if (Towers.Contains(selectedTower))
        {
            Select(false, selectedTower);
            return true;
        }

        return false;
    }
    
    public virtual void Unsubscribe()
    {
        NetworkEventbus.InputEvents.OnObjectClicked -= GetTower;
    }


}
