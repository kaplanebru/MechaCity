using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public List<Tower> selectionGroup = new ();
    public int maxTowersInGroup;
    public Enums.TurnState state;
    
    private void OnEnable()
    {
        Eventbus.TowerEvents.OnTowerClicked += TowerClicked;
    }

    void SelectTower(bool select,Tower newTower)
    {
        newTower.SetColor(select ? newTower.Data.TeamData.SelectedMaterial :  newTower.Data.TeamData.DefaultMaterial);

        if(select)
            selectionGroup.Add(newTower);
        else
            selectionGroup.Remove(newTower);
    }
    
    private void TowerClicked(Tower newTower)
    {
        //if not shown, show chain
        if (SelectedTwice(newTower)) return;
            
        if(selectionGroup.Count == maxTowersInGroup)
            ResetSelectionGroup();
        
        SelectTower(true, newTower);
        //chain position will be on selectionGroup[0]
        //if more towers in the group, stretch chain
    }
    
    public void SelectionEnded()
    {
        state = Enums.TurnState.BoundState;
        //Enable Bound Towers Script
        Eventbus.TurnEvents.OnSelectionEnded?.Invoke(selectionGroup);
        selectionGroup.Clear();
        //Disable or Disappear GroupTowersButton
    }

    bool SelectedTwice(Tower newTower)
    {
        if (selectionGroup.Count == 0) 
            return false;

        if (!selectionGroup.Contains(newTower)) 
            return false;
        
        SelectTower(false, newTower);
        return true;
    }

    void ResetSelectionGroup()
    {
        for (int i = 0; i < maxTowersInGroup; i++)
        {
            SelectTower(false, selectionGroup[0]);
        }
    }
  
    private void OnDisable()
    {
        Eventbus.TowerEvents.OnTowerClicked -= TowerClicked;
    }

}
