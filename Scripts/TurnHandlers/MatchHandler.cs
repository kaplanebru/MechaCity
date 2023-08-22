using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Datas;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MatchData : BaseTurnData //bizim sonrakine göndereceğimiz
{
    public List<TowerGridRelationModel> DeadTowerGridPairs = new();
}

public class MatchHandler : BaseTurnHandler, ITurnActionHandler<MatchData>
{
    public MatchData Data { get; private set; }

    public override void OnHandlerEnabled()
    {
        Data = new();
    }

    public override void ProcessTransferredData(BaseTurnData data)
    {
        var incomingData = (FireData) data;
        Data.DeadTowerGridPairs = incomingData.DeadTowers;
    }

    public override void Setup()
    {
        RematchTowers();
    }

    void LinkTowers(Tower tower1, Tower tower2)
    {
        if (!tower1.Data.LinkedTowers.Contains(tower2))
            tower1.Data.LinkedTowers.Add(tower2);
    }

    void RemoveLink(Tower tower1, Tower tower2)
    {
        tower1.Data.LinkedTowers.Remove(tower2);
    }
    
    int CheckSlot(int number, GameGrid grid, Tower detachedTower)
    {
        if (number is >= 0 and < GameGrid.SlotAmount)
        {
            var slot = grid.Slots[number];
            if (slot.HasTower)
            {
                LinkTowers(slot.Tower, detachedTower);
                return 1;
            }
        }
        return 0;
    }
    
    void RestoreDetachedTowersOfDeadTower(TowerGridRelationModel deadTowerGridModel, Tower detachedTower)
    {
        int deadTowerId = deadTowerGridModel.Tower.Data.Id;

        for (int i = 0; i < GameGrid.SlotAmount - 1; i++)
        {
            int counter = 0;
            
            counter += CheckSlot(deadTowerId - i, deadTowerGridModel.Grid, detachedTower);
            counter += CheckSlot(deadTowerId + i, deadTowerGridModel.Grid, detachedTower);

            if (counter > 0) break;
        }
    }
    void RematchTowers()
    {
        foreach (var deadTowerGridModel in Data.DeadTowerGridPairs)
        {
            var deadTower = deadTowerGridModel.Tower;
            for (var i = deadTower.Data.LinkedTowers.Count - 1; i >= 0; i--)
            {
                RemoveLink(deadTower, deadTower.Data.LinkedTowers[i]);
                RestoreDetachedTowersOfDeadTower(deadTowerGridModel, deadTower.Data.LinkedTowers[i]);
            }
        }
    }

    public override void Unsubscribe()
    {
    }
}