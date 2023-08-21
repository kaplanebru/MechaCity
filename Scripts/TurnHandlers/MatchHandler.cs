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
        //ConstantData.DeadTowerGridPairs.ForEach();
        //TODO: dead tower'a linked olanları bul, othergrid 2 taraftan da biri olabilir, yani dead tower kimlerden bilmemiz lazım
    }

    public void RestoreDetachedTowersOfDeadTowers()
    {
        foreach (var deadTowerGridModel in Data.DeadTowerGridPairs)
        {
            for (var i = deadTowerGridModel.Tower.Data.LinkedTowers.Count - 1; i >= 0; i--)
            {
                var linkedTower = deadTowerGridModel.Tower.Data.LinkedTowers[i];
                
                RemoveLink(linkedTower, deadTowerGridModel.Tower);
                RestoreDetachedTowers(deadTowerGridModel, linkedTower);
            }
        }
    }

    void LinkTowers(Tower tower1, Tower tower2)
    {
        if (!tower1.Data.LinkedTowers.Contains(tower2))
            tower1.Data.LinkedTowers.Add(tower2);
    }

    void RemoveLink(Tower tower1, Tower tower2)
    {
        tower1.Data.LinkedTowers.Remove(tower2);//bulletin vurulduğu yerden de gelebilir
    }

    void RestoreDetachedTowers(TowerGridRelationModel deadTowerGridModel, Tower detachedTower)
    {
        int deadTowerId = deadTowerGridModel.Tower.Data.Id;

        for (int i = 0; i < GameGrid.SlotAmount - 1; i++)
        {
            int counter = 0;
            
            counter += CheckLinkPossibility(deadTowerId - i, deadTowerGridModel, detachedTower);
            counter += CheckLinkPossibility(deadTowerId + i, deadTowerGridModel, detachedTower);

            if (counter > 0) break;
        }
    }

    int CheckLinkPossibility(int number, TowerGridRelationModel deadTowerGridModel, Tower detachedTower)
    {
        if (number >= 0 && number < GameGrid.SlotAmount)
        {
            if (deadTowerGridModel.Grid.Slots[number].HasTower) //bu koşulu kontrol etmeye gerek olmayabilir
            {
                LinkTowers(detachedTower, deadTowerGridModel.Grid.Slots[number].Tower);
                return 1;
            }
        }
        return 0;
    }

    public override void Unsubscribe()
    {
    }
}

public class TowerGridRelationModel
{
    public GameGrid Grid { get; }
    public Tower Tower { get; }
    public TowerGridRelationModel(GameGrid grid, Tower tower)
    {
        Grid = grid;
        Tower = tower;
    }

}