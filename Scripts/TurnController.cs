using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public List<Tower> towerGroup = new ();
    public int maxTowersInGroup;
    public Enums.TurnState state;
    
    private void OnEnable()
    {
        Eventbus.TowerEvents.OnTowerClicked += TowerClicked;
    }

    void SelectTower(Tower newTower, bool select)
    {
        newTower.SetColor(select ? newTower.Data.TeamData.SelectedMaterial :  newTower.Data.TeamData.DefaultMaterial);

        if(select)
            towerGroup.Add(newTower);
        else
            towerGroup.Remove(newTower);
    }
    
    private void TowerClicked(Tower newTower)
    {
        //if not shown, show chain
        if (SelectedTwice(newTower)) return;
            
        if(towerGroup.Count == maxTowersInGroup)
            ResetTowerGroup();
        
        SelectTower(newTower, true);
        //chain position will be on towerGroup[0]
        //if more towers in the group, stretch chain
    }
    
    public void SelectionEnded()
    {
        state = Enums.TurnState.BoundState;
        //Enable Bound Towers Script
        Eventbus.TurnEvents.OnSelectionEnded?.Invoke(towerGroup);
        towerGroup.Clear();
        //Disable or Disappear GroupTowersButton
    }

    bool SelectedTwice(Tower newTower)
    {
        if (towerGroup.Count == 0) 
            return false;

        if (towerGroup.Last() != newTower) 
            return false;
        
        SelectTower(towerGroup.Last(), false);
        return true;
    }

    void ResetTowerGroup()
    {
        for (int i = 0; i < maxTowersInGroup; i++)
        {
            SelectTower(towerGroup[0], false);
        }
    }
  
    private void OnDisable()
    {
        Eventbus.TowerEvents.OnTowerClicked -= TowerClicked;
    }

}
