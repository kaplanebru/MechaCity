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
public class TowerGroupData : BaseTurnTransferData
{
    public List<Tower> TowerGroup = new();
}

public class TowerGroupHandler : BaseTurnHandler, ITurnActionHandler<TowerGroupData>
{
    public TowerGroupData TransferData { get; private set; }

    public override TurnHandlerType HandlerType => TurnHandlerType.TowerGroup;

    public override void OnHandlerEnabled()
    {
        TransferData = new();
        Eventbus.InputEvents.OnObjectClicked += TowerSelected;
    }
    
    public override void ProcessIncomingData(BaseTurnTransferData data) //(params object[] args)
    {
        var incomingData = (SelectionData) data;
        TransferData.TowerGroup = incomingData.SelectionGroup;
    }

    public override void Setup() {}

    private void TowerSelected(params object[] args)
    {
        var tower = args[0] as Tower;
        if (tower == null) return;

        if (!TransferData.TowerGroup.Contains(tower)) return;
        RiseAndFall(tower, 1, true);
    }

    void RiseAndFall(Tower selectedTower, float amount, bool rise)
    {
        foreach (var tower in TransferData.TowerGroup)
        {
            if (tower == selectedTower)
                tower.towerParts.ChangeHeight(tower.Data.Height += amount);
            else
                tower.towerParts.ChangeHeight(tower.Data.Height -= amount / (TransferData.TowerGroup.Count - 1));
        }
    }

    public override void Unsubscribe()
    {
        Eventbus.InputEvents.OnObjectClicked -= TowerSelected;
    }

    void ResetGroups()
    {
        TransferData.TowerGroup.Clear();
    }
}