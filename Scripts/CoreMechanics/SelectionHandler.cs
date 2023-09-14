using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;
using Object = UnityEngine.Object;


[Serializable]
public class SelectionData : BaseTurnData
{
    public List<Tower> SelectionGroup = new();
    public int MaxTowersInGroup = 2;
}

public class SelectionHandler : BaseTurnHandler, ITurnActionHandler<SelectionData>
{
    public SelectionData Data { get; private set; }

    public override TurnHandlerType HandlerType => TurnHandlerType.Selection;

    public override void OnHandlerEnabled()
    {
        Data = new();
        Data.SelectionGroup.Clear();
        Eventbus.InputEvents.OnObjectClicked += TowerPartClicked;
    }
    
    private void TowerPartClicked(params object[] args)
    {

        var tower = args[0] as Tower;
        if (tower == null) return;
        
       // tower.transform.localScale = new Vector3(2, tower.transform.localScale.y, tower.transform.localScale.z);

        
        if (tower.Data.TeamTowerData.TeamType == teams["rivalTeam"].Data.TeamTowerData.TeamType) return;
        if (SelectedTwice(tower)) return;
        
        if (Data.SelectionGroup.Count == Data.MaxTowersInGroup)
            ResetSelectionGroup();
        
        AddToSelection(true, tower);
    }

    public override void Setup()
    {
        ManageCompleteButton(false);
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
        Eventbus.InputEvents.OnObjectClicked -= TowerPartClicked;
    }
}