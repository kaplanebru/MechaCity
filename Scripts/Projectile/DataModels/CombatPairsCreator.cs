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
    List<CombatPair> _combatPairs;

    public CombatPairsCreator(List<CombatPair> combatPairs)
    {
        _combatPairs = combatPairs;
    }
    
    public void CreateCombatPairs(Dictionary<uint, ActorData> tempActors, bool isReversed = false)
    {
        List<uint> actorIDs = tempActors.Keys.ToList();
        if (isReversed)
        {
            var firstID = actorIDs.First();
            actorIDs.RemoveAt(0);
            actorIDs.Reverse();
            actorIDs.Insert(0, firstID);
        }

        _combatPairs.Clear();
        Eventbus.LinkEvents.OnCreatingCombatPairs?.Invoke(actorIDs);
        actorIDs.ForEach(id => CombatPairByTower(tempActors[id]));
    }
    
   

    public void CombatPairByTower(ActorData mainActor)
    {
        //OrderLinkedTowersByID(tower); //todo
        
        var linkedActors = mainActor.LinkedActors;
        foreach (var id in linkedActors)
        {
            var linkedActor = ActorHolder.Registry[id];
            AddToPair(mainActor, linkedActor);
        }
    }

    void AddToPair(ActorData actor1, ActorData actor2)
    {
        _combatPairs.Add(new CombatPair(actor1, actor2));
    }

    void OrderLinkedTowersByID(TowerData tower)
    {
        //TODO: NEDEN SLOT ID? uzaklık için mi. SlotId towers'daki sıralama olarak set edilebilir!
        // tower.LinkedTowerIDs =
        //     tower.LinkedTowerIDs.OrderBy(other => Mathf.Abs(tower.SlotId - AllTowers.GetData(other).SlotId))
        //         .ToList();
    }
}