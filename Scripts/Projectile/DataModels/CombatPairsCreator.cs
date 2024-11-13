using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using DataModels;
using Health;
using Towers;
using UnityEngine;

public class CombatPairsCreator
{
    Dictionary<uint,List<CombatPair>> _combatPairs;

    public CombatPairsCreator(Dictionary<uint,List<CombatPair>> combatPairs)
    {
        _combatPairs = combatPairs;
    }
    
    public void CreateCombatPairs(List<uint> tempActors, bool isReversed = false)
    {
        _combatPairs.Clear();
        tempActors.ForEach(id => CombatPairByActor(ActorHolder.Registry[id], isReversed));
    }
    
    public void CombatPairByActor(ActorData mainActor, bool isReversed = false)
    {
        //OrderTargetTowersByID(tower); //todo: birden fazla linked varsa diye, in further update
        
        var targetActors = mainActor.TargetActors;
        foreach (var id in targetActors)
        {
            var targetActor = ActorHolder.Registry[id];
            var pair = AddToPair(mainActor, targetActor);
            pair.OrderTowers(isReversed);
        }
    }

    CombatPair AddToPair(ActorData actor1, ActorData actor2)
    {
        var pair = new CombatPair(actor1, actor2);
        
        if(!_combatPairs.ContainsKey(actor1.ID))
            _combatPairs.Add(actor1.ID, new List<CombatPair> {pair});
        else
            _combatPairs[actor1.ID].Add(pair);

        return pair;
    }

    void OrderTargetTowersByID(TowerData tower)
    {
        //TODO: NEDEN SLOT ID? uzaklık için mi. SlotId towers'daki sıralama olarak set edilebilir!
        // tower.LinkedTowerIDs =
        //     tower.LinkedTowerIDs.OrderBy(other => Mathf.Abs(tower.SlotId - AllTowers.GetData(other).SlotId))
        //         .ToList();
    }
}