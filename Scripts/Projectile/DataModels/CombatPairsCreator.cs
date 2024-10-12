using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
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
       // TowersRelationManager.SetLinkedTowers(tempTowers);
        Eventbus.LinkEvents.OnCreatingCombatPairs?.Invoke( tempActors.Select(t => t).ToList());
        tempTowers.ForEach(CombatPairByTower);
    }
    
   

    public void CombatPairByTower(TowerData tower)
    {
        //OrderLinkedTowersByID(tower); //todo

        var linkedTowers = ActorManager.GetLinksByID(tower.UniqID);
        foreach (var id in linkedTowers)
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
        //TODO: NEDEN SLOT ID? uzaklık için mi. SlotId towers'daki sıralama olarak set edilebilir!
        // tower.LinkedTowerIDs =
        //     tower.LinkedTowerIDs.OrderBy(other => Mathf.Abs(tower.SlotId - AllTowers.GetData(other).SlotId))
        //         .ToList();
    }
}