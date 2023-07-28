using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Datas;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

public class TowerGroupData : BaseTurnData
{
    public string test;
    public List<Tower> TowerGroup = new();
}

public class TowerGroupHandler : BaseTurnHandler, ITurnActionHandler<TowerGroupData>
{
    public TowerGroupData Data { get; private set; }
    
    public override void Subscribe()
    {
        Data = new();
        Eventbus.TowerEvents.OnTowerClicked += TowerSelected;
    }

    public override void ProcessTransferredData(BaseTurnData data) //(params object[] args)
    {
        var incomingData = (SelectionData)data;
        Data.TowerGroup = incomingData.SelectionGroup;
    }
    private void TowerSelected(Tower tower)
    {
        if (!Data.TowerGroup.Contains(tower)) return;
        //check lean input for bool
        RiseAndFall(tower, 1,true);
    }

    void RiseAndFall(Tower selectedTower, float amount, bool rise)
    {
        foreach (var tower in Data.TowerGroup)
        {
            if (tower == selectedTower)
                tower.transform.DOScaleY(tower.Data.Height += amount, 1);
            else
                tower.transform.DOScaleY(tower.Data.Height -= amount/(Data.TowerGroup.Count-1), 1);
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
        Data.TowerGroup.Clear();
    }


}
