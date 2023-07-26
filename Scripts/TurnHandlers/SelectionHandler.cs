using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class SelectionData : BaseTransferData
{
    public List<Tower> selectionGroup = new ();
}

public class SelectionHandler : BaseTurnHandler, ITurnActionHandler<SelectionData>
{
    public SelectionData TransferData { get; set; }
    public void ProcessTransferredData(BaseTransferData transferData)
    {
    }

    private List<Tower> selectionGroup = new ();
    public int maxTowersInGroup;
    
    public override void Subscribe()
    {
        TransferData = new();
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

    public override void SetTransferData()
    {
        TransferData.selectionGroup = selectionGroup;
    }

    public void SelectionEnded()
    {
        CompleteAction();
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


    public SelectionData transferData { get; set; }
}
