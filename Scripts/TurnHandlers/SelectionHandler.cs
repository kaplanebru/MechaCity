using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

[Serializable]
public class SelectionData : BaseTurnData
{
    public List<Tower> SelectionGroup = new();
    public int MaxTowersInGroup = 2;
}

public class SelectionHandler : BaseTurnHandler, ITurnActionHandler<SelectionData>
{
    //learn how to serialize interface
    public SelectionData Data { get; private set; }

    public override void OnHandlerEnabled()
    {
        Data = new();
        Data.SelectionGroup.Clear();
        Eventbus.TowerEvents.OnTowerClicked += TowerClicked;
    }

    public override void Setup()
    {
        teams["currentTeam"].SetClickability(true);
        teams["rivalTeam"].SetClickability(false);
        ManageCompleteButton(false);
    }

    private void TowerClicked(Tower newTower)
    {
        //if not shown, show chain
        if (SelectedTwice(newTower)) return;

        if (Data.SelectionGroup.Count == Data.MaxTowersInGroup)
            ResetSelectionGroup();

        AddToSelection(true, newTower);


        //chain position will be on selectionGroup[0]
        //if more towers in the group, stretch chain
    }

    void AddToSelection(bool select, Tower newTower)
    {
        newTower.towerParts.SetColor(select ? teams["currentTeam"].Data.TeamTowerData.SelectedMaterial : teams["currentTeam"].Data.TeamTowerData.DefaultMaterial);

        if (select)
            Data.SelectionGroup.Add(newTower);
        else
            Data.SelectionGroup.Remove(newTower);

        ManageCompleteButton(Data.SelectionGroup.Count == Data.MaxTowersInGroup);
    }

    void ManageCompleteButton(bool enable)
    {
        Eventbus.UIEvents.OnButtonCall?.Invoke(enable);
    }

    public void SelectionCompleted()
    {
        CompleteAction();
    }

    bool SelectedTwice(Tower newTower)
    {
        if (Data.SelectionGroup.Contains(newTower))
        {
            AddToSelection(false, newTower);
            return true;
        }
        return false;
    }

    void ResetSelectionGroup()
    {
        for (int i = 0; i < Data.MaxTowersInGroup; i++)
        {
            AddToSelection(false, Data.SelectionGroup[0]);
        }
    }

    public override void Unsubscribe()
    {
        Eventbus.TowerEvents.OnTowerClicked -= TowerClicked;
    }
}