using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireData : BaseTurnData
{
    public List<Pairs> FightingPairs = new();
}

public class FireHandler : BaseTurnHandler, ITurnActionHandler<FireData>
{
    public FireData Data { get; private set; }

    public override void Subscribe()
    {
        Data = new(); //Startta yapılabilir
        Data.FightingPairs.Clear();
        Eventbus.FireEvents.OnPairsOrdered += AddToPairsList;
    }

    private void AddToPairsList(Pairs newPair)
    {
        Data.FightingPairs.Add(newPair);
    }

    void Fire()
    {
        foreach (var pair in Data.FightingPairs)
        {
            pair.Perpetrator.Attack(pair.Victim);
        }
    }

    // void Fire()
    // {
    //     foreach (var tower in currentPlayer.Data.Towers)
    //     {
    //         tower.Fight();
    //     }
    // }
    
    // void Fight(Tower tower1, Tower tower2)
    // {
    //     if (tower1.Data.Height > tower2.Data.Height)
    //         tower2.Descend(tower1.Data.AttackAmount);
    //     
    //     else
    //         tower1.Descend(tower2.Data.AttackAmount);
    // }



    public override void Unsubscribe()
    {
        Eventbus.FireEvents.OnPairsOrdered -= AddToPairsList;
    }
}
