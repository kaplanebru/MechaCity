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
    Dictionary<uint,List<CombatPair>> _pairGroupsByActor = new();
    private Dictionary<int, CombatPair> _allPairs = new();
    
    public  (Dictionary<uint,List<CombatPair>>, Dictionary<int, CombatPair>) CreateCombatPairs(List<uint> tempActors, bool isReversed = false)
    {
        _allPairs.Clear();
        _pairGroupsByActor.Clear();
        tempActors.ForEach(id => CombatPairByActor(ActorDB.Registry[id], isReversed));
        return (_pairGroupsByActor, _allPairs);
       
    }
    
    public void CombatPairByActor(ActorData mainActor, bool isReversed = false)
    {
        //OrderTargetTowersByID(tower); //todo: birden fazla linked varsa diye, in further update
        
        var targetActors = mainActor.TargetActors;
        foreach (var id in targetActors)
        {
            var targetActor = ActorDB.Registry[id];
            var pair = AddToPair(mainActor, targetActor);
            pair.OrderTowersByGridDirection(isReversed);
        }
    }

    CombatPair AddToPair(ActorData actor1, ActorData actor2)
    {
      
        var pair = new CombatPair(actor1, actor2);
        pair.ID = UniqueIdGenerator.IntId();
        
        if(!_pairGroupsByActor.ContainsKey(actor1.ID))
            _pairGroupsByActor.Add(actor1.ID, new List<CombatPair> {pair});
        else
            _pairGroupsByActor[actor1.ID].Add(pair);
        
        _allPairs.Add(pair.ID, pair);
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