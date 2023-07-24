using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Datas;
using DG.Tweening;
using UnityEngine;

public class BoundTowers : MonoBehaviour
{
    [SerializeField] private List<Tower> towerGroup = new();
    private void OnEnable()
    {
        Eventbus.TurnEvents.OnSelectionEnded += BoundingTowers;
        Eventbus.TowerEvents.OnTowerSelected += TowerSelected;
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

    private void BoundingTowers(List<Tower> towers)
    {
        towerGroup.AddRange(towers);
    }

    private void OnDisable()
    {
        Eventbus.TurnEvents.OnSelectionEnded -= BoundingTowers;
        Eventbus.TowerEvents.OnTowerSelected -= TowerSelected;
    }

    void ResetGroups()
    {
        towerGroup.Clear();
    }
}
