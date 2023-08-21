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
    public Dictionary<GameGrid, Tower> DeadTowerGridPairs = new();

    public List<Tower> DetachedTowers = new();
    //yeni matchleri gönderebilir. Gerçi bunlar zaten slotun ya da towerın kendisinde ekli.
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

    void RematchOrphanTowers()
    {
        GetDetachedTowers();
    }

    void GetDetachedTowers()
    {
        foreach (var deadTower in Data.DeadTowerGridPairs)
        {
            foreach (var detachedTower in deadTower.Value.Data.LinkedTowers)
            {
                RestoreLinkedTowers(deadTower.Value, detachedTower, deadTower.Key);
            }
            //Data.DetachedTowers.AddRange(deadTower.Data.LinkedTowers.Except(Data.DetachedTowers));
        }
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

    void RestoreLinkedTowers(Tower detachedTower, Tower deadTower, GameGrid otherGrid)
    {
        // //if (!slot.HasTower) return;

        RemoveLink(detachedTower, deadTower);
        int deadTowerId = deadTower.Data.Id;

        for (int i = 0; i < GameGrid.SlotAmount - 1; i++)
        {
            int counter = 0;

            int number = deadTowerId - i;
            if (number >= 0) //&& number < GameGrid.SlotAmount
            {
                if (otherGrid.Slots[number].HasTower)
                {
                    LinkTowers(detachedTower, otherGrid.Slots[number].Tower);
                    counter++;
                }
            }

            number = deadTowerId + i;
            if (number < GameGrid.SlotAmount)
            {
                if (otherGrid.Slots[number].HasTower)
                {
                    LinkTowers(detachedTower, otherGrid.Slots[number].Tower);
                    counter++;
                }
            }

            if (counter > 0) break;
        }
    }

    public override void Unsubscribe()
    {
    }
}

public class GridTowerRelationModel
{
    private Tower Tower;
    private GameGrid Grid;

    GridTowerRelationModel(GameGrid grid, Tower tower)
    {
        Grid = grid;
        Tower = tower;
    }
}