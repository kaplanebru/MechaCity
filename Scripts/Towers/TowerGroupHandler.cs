using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Datas;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

public class TowerGroupHandler : BaseTurnHandler, ITurnHandler
{
    [SerializeField] private List<Tower> towerGroup = new();
    public override void Subscribe()
    {
        Eventbus.TurnEvents.OnTurnStateChanged += CreateTowerGroups;
        Eventbus.TowerEvents.OnTowerClicked += TowerSelected;
    }
    
    private void CreateTowerGroups(params object[] args)
    {
        var towers = ((TurnManager)args[0]).currentTowerGroup;
        towerGroup.AddRange(towers);
    }

    private void TowerSelected(Tower tower)
    {
        //listede değilse selectible değil yapılabilir diğer hepsi
        //turn state'e bakılabilir
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


    public override void Unsubscribe()    
    {
        Eventbus.TurnEvents.OnTurnStateChanged -= CreateTowerGroups;
        Eventbus.TowerEvents.OnTowerClicked -= TowerSelected;
    }

    void ResetGroups()
    {
        towerGroup.Clear();
    }
}
