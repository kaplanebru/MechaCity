using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;


public class SelectionHandler : BaseTurnHandler, ITurnActionHandler
{
    public List<Tower> selectionGroup = new ();
    public int maxTowersInGroup;
    
    public override void Subscribe()
    {
        selectionGroup.Clear();
        Eventbus.TowerEvents.OnTowerClicked += TowerClicked;
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
    
    void SelectTower(bool select,Tower newTower)
    {
        newTower.SetColor(select ? newTower.Data.TeamData.SelectedMaterial :  newTower.Data.TeamData.DefaultMaterial);

        if(select)
            selectionGroup.Add(newTower);
        else
            selectionGroup.Remove(newTower);
    }

    public void SelectionEnded()
    {
        Eventbus.TurnEvents.OnSelectionEnded?.Invoke(selectionGroup);
        CompleteAction();
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
  
    public override void Unsubscribe()    
    {
        Eventbus.TowerEvents.OnTowerClicked -= TowerClicked;
    }

    public void PlayTurnAction()
    {
        
    }
}
