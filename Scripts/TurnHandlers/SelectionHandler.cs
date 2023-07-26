using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class SelectionData : BaseTurnData
{
    public List<Tower> SelectionGroup = new ();
    public int MaxTowersInGroup;
}

public class SelectionHandler : BaseTurnHandler, ITurnActionHandler<SelectionData>
{
    public SelectionData Data { get; private set; }
    
    public override void Subscribe()
    {
        Data = new();
        Data.SelectionGroup.Clear();
        Eventbus.TowerEvents.OnTowerClicked += TowerClicked;
    }

    private void TowerClicked(Tower newTower)
    {
        //if not shown, show chain
        if (SelectedTwice(newTower)) return;
            
        if(Data.SelectionGroup.Count == Data.MaxTowersInGroup)
            ResetSelectionGroup();
        
        SelectTower(true, newTower);
        //chain position will be on selectionGroup[0]
        //if more towers in the group, stretch chain
    }
    
    void SelectTower(bool select,Tower newTower)
    {
        newTower.SetColor(select ? newTower.Data.TeamData.SelectedMaterial :  newTower.Data.TeamData.DefaultMaterial);

        if(select)
            Data.SelectionGroup.Add(newTower);
        else
            Data.SelectionGroup.Remove(newTower);
    }



    public void SelectionEnded()
    {
        CompleteAction();
    }

    bool SelectedTwice(Tower newTower)
    {
        if (Data.SelectionGroup.Count == 0) 
            return false;

        if (!Data.SelectionGroup.Contains(newTower)) 
            return false;
        
        SelectTower(false, newTower);
        return true;
    }

    void ResetSelectionGroup()
    {
        for (int i = 0; i < Data.MaxTowersInGroup; i++)
        {
            SelectTower(false, Data.SelectionGroup[0]);
        }
    }
  
    public override void Unsubscribe()    
    {
        Eventbus.TowerEvents.OnTowerClicked -= TowerClicked;
    }
}
