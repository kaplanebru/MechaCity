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
}

public class TowerGroupHandler : BaseTurnHandler, ITurnActionHandler<TowerGroupData>
{
    public TowerGroupData Data { get; private set; }

    public override TurnHandlerType HandlerType => TurnHandlerType.TowerGroup;

    public override void OnHandlerEnabled()
    {
        Data = new();
        Eventbus.InputEvents.OnObjectClicked += TowerSelected;
    }
    
    public override void ProcessIncomingData(BaseTurnData data) //(params object[] args)
    {
        var incomingData = (SelectionData) data;
        Data.TowerGroup = incomingData.SelectionGroup;
    }

    public override void Setup() {}

    private void TowerSelected(params object[] args)
    {
        var tower = args[0] as Tower;
        if (tower == null) return;

        if (!Data.TowerGroup.Contains(tower)) return;
        RiseAndFall(tower, 1, true);
    }

    void RiseAndFall(Tower selectedTower, float amount, bool rise)
    {
        foreach (var tower in Data.TowerGroup)
        {
            if (tower == selectedTower)
                tower.towerParts.ChangeHeight(tower.Data.Height += amount);
            else
                tower.towerParts.ChangeHeight(tower.Data.Height -= amount / (Data.TowerGroup.Count - 1));
        }
    }

    public void ActionEnded()
    {
        CompleteAction();
    }

    public override void Unsubscribe()
    {
        Eventbus.InputEvents.OnObjectClicked -= TowerSelected;
    }

    void ResetGroups()
    {
        Data.TowerGroup.Clear();
    }
}