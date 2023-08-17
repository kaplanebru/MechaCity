using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class FireData : BaseTurnData
{
    public List<CombatPair> CombatPairs = new();
}

public class FireHandler : BaseTurnHandler, ITurnActionHandler<FireData>
{
    public FireData Data { get; private set; }

    public override void Subscribe()
    {
        Data = new(); //Startta yapılabilir
        Data.CombatPairs.Clear(); //sadece değişenlerin reoder edildiği dinamik bir sistem yapılabilir
        
        Eventbus.FireEvents.OnPairsOrdered += AddToFightingPairsList;
        Eventbus.FireEvents.OnFireEnabled.Invoke();
    }

    private void AddToFightingPairsList(CombatPair newPair)
    {
        Data.CombatPairs.Add(newPair);
    }

    void Fire()
    {
        Data.CombatPairs.ForEach(p=>p.Shoot());
    }
    
    public override void Unsubscribe()
    {
        Eventbus.FireEvents.OnPairsOrdered -= AddToFightingPairsList;
    }
}
