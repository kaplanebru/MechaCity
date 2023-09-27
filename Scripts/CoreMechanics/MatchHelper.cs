using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHelper : MonoBehaviour
{
    //relate to combat handler

    private void OnEnable()
    {
        Eventbus.FireEvents.OnTowerGridDetection += HandleDeadTower;
    }

    private void HandleDeadTower(TowerGridRelationModel deadTowerGridModel)
    {
        var deadTower = deadTowerGridModel.Tower;
        for (var i = deadTower.Data.LinkedTowers.Count - 1; i >= 0; i--)
        {
            var linkedTower = deadTower.Data.LinkedTowers[i];

            RematchDetachedTowers(deadTowerGridModel, linkedTower);
            RemoveLink(deadTower, linkedTower); //önce de gelebilir

            if (deadTower.Data.TeamTowerData.TeamType != linkedTower.Data.TeamTowerData.TeamType) //bug fix: birden fazla vurulmuşsa gerek yok
                SwitchSides(deadTower);
        }
    }

    void RematchDetachedTowers(TowerGridRelationModel deadTowerGridModel, Tower detachedTower)
    {
        int deadTowerSlotId = deadTowerGridModel.Tower.Data.SlotId;

        for (int i = 1; i < GameGrid.SlotAmount - 1; i++)
        {
            int linkCounter = 0;

            linkCounter += CheckSlotForLink(deadTowerSlotId - i, deadTowerGridModel.Grid, detachedTower);
            linkCounter += CheckSlotForLink(deadTowerSlotId + i, deadTowerGridModel.Grid, detachedTower);

            if (linkCounter > 0) break;
        }
    }
    
    int CheckSlotForLink(int number, GameGrid grid, Tower detachedTower)
    {
        if (number is >= 0 and < GameGrid.SlotAmount)
        {
            var slot = grid.Slots[number];
            if (slot.HasTower)
            {
                if (slot.Tower.Data.TeamTowerData.TeamType == detachedTower.Data.TeamTowerData.TeamType) //bug fix: karşıdaki tower aynı team'dense pas
                    return 0;
                
                LinkTowers(slot.Tower, detachedTower);
                return 1;
            }
        }
        return 0;
    }

    void LinkTowers(Tower tower1, Tower tower2)
    {
        if (!tower1.Data.LinkedTowers.Contains(tower2))
            tower1.Data.LinkedTowers.Add(tower2);

        if (!tower2.Data.LinkedTowers.Contains(tower1)) //bug fix: hem sağı gem solu alsın diye deneme
            tower2.Data.LinkedTowers.Add(tower1);
    }

    void RemoveLink(Tower deadTower, Tower otherTower)
    {
        deadTower.Data.LinkedTowers.Remove(otherTower);
        otherTower.Data.LinkedTowers.Remove(deadTower);
    }

    void SwitchSides(Tower deadTower)
    {
        Eventbus.TeamEvents.OnTeamChange?.Invoke(deadTower);
    }

    private void OnDisable()
    {
        Eventbus.FireEvents.OnTowerGridDetection -= HandleDeadTower;
    }

    //TODO: STAR VE ONDİSABLE'a event listener eklenmişse düzelt. Unsubscireda da olabilir
}