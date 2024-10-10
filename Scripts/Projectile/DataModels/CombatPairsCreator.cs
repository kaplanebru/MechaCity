using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Towers;
using UnityEngine;

public class CombatPairsCreator
{
    List<CombatPair> _combatPairs;

    public CombatPairsCreator(List<CombatPair> combatPairs)
    {
        _combatPairs = combatPairs;
    }
    
    public void CreateCombatPairs(List<TowerData> tempTowers, bool isReversed = false)
    {
        if (isReversed)
        {
            tempTowers.Add(tempTowers[0]);
            tempTowers.RemoveAt(0);
            tempTowers.Reverse();
        }

        _combatPairs.Clear();
        LinkPairs(tempTowers);
        tempTowers.ForEach(CombatPairByTower);
    }
    
    private void LinkPairs(List<TowerData> towers) //ters de gelebilir
    {
        for (var i = 0; i < AllTowers.TowersCount; i++)
        {
            towers[i].LinkedTowerIDs.Clear();

            int next = towers[(i + 1) % AllTowers.TowersCount].UniqID; //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da
            towers[i].LinkedTowerIDs.Add(next);
                
            // print("index: " + (i + 1) % TowersCount + " id: " + next);
        }
    }

    public void CombatPairByTower(TowerData tower)
    {
        OrderLinkedTowersByID(tower);

        foreach (var id in tower.LinkedTowerIDs)
        {
            var linkedTower = AllTowers.GetData(id);
            AddToPair(tower, linkedTower);
        }
    }

    void AddToPair(TowerData tower1, TowerData tower2)
    {
        _combatPairs.Add(new CombatPair(tower1, tower2));
    }

    void OrderLinkedTowersByID(TowerData tower)
    {
        tower.LinkedTowerIDs =
            tower.LinkedTowerIDs.OrderBy(other => Mathf.Abs(tower.SlotId - AllTowers.GetData(other).SlotId))
                .ToList();
    }
}