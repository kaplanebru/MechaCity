using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Datas;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

public class TowerGroupData : BaseTransferData
{
    public string test;
}

public class TowerGroupHandler : BaseTurnHandler<TowerGroupData>, ITurnActionHandler
{
    [SerializeField] private List<Tower> towerGroup = new();
    public override void Subscribe()
    {
        Eventbus.TowerEvents.OnTowerClicked += TowerSelected;
    }

    public override void ProcessTransferredData(params object[] args) //(params object[] args)
    {
        var incomingData = (SelectionData)args[0];
        towerGroup = incomingData.selectionGroup;
    }
    private void TowerSelected(Tower tower)
    {
        if (!towerGroup.Contains(tower)) return;
        //check lean input for bool
        RiseAndFall(tower, 1,true);
    }

    void RiseAndFall(Tower selectedTower, float amount, bool rise)
    {
        foreach (var tower in towerGroup)
        {
            if (tower == selectedTower)
                tower.transform.DOScaleY(tower.Data.Height += amount, 1);
            else
                tower.transform.DOScaleY(tower.Data.Height -= amount/(towerGroup.Count-1), 1);
        }
    }

    public void ActionEnded()
    {
        CompleteAction();
    }


    public override void Unsubscribe()    
    {
        Eventbus.TowerEvents.OnTowerClicked -= TowerSelected;
    }

    void ResetGroups()
    {
        towerGroup.Clear();
    }

  
}
