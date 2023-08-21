using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Datas;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class TowerGroupData : BaseTurnData
{
    public List<Tower> TowerGroup = new();
    
    //pairi tutalım towerda
    //tower id listesi de olabilir slotları yollamak için
}

public class TowerGroupHandler : BaseTurnHandler, ITurnActionHandler<TowerGroupData>
{
    public TowerGroupData Data { get; private set; }
    
    public override void OnHandlerEnabled()
    {
        Data = new();
        Eventbus.TowerEvents.OnTowerClicked += TowerSelected;
    }

   

    public override void ProcessTransferredData(BaseTurnData data) //(params object[] args)
    {
        var incomingData = (SelectionData)data;
        Data.TowerGroup = incomingData.SelectionGroup;
    }
    
    public override void Setup() {}
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
                tower.transform.DOScaleY(tower.height += amount, 1);
            else
                tower.transform.DOScaleY(tower.height -= amount/(Data.TowerGroup.Count-1), 1);
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
