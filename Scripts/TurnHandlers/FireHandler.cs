using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

public class FireData : BaseTurnData
{
    public List<CombatPair> CombatPairs = new();
    public List<Tower> AlteredTowers = new();
}

public class FireHandler : BaseTurnHandler, ITurnActionHandler<FireData>
{
    public FireData Data { get; private set; }

    public override void Subscribe()
    {
        Data = new(); //Startta yapılabilir
        
        RemoveAlteredCombatPairs();
        Eventbus.FireEvents.OnPairsAltered += AddToCombatPairs;

        Fire();
    }
    
    public override void ProcessTransferredData(BaseTurnData data) //(params object[] args)
    {
        var incomingData = (TowerGroupData)data;
        Data.AlteredTowers = incomingData.TowerGroup;
    }

    private void AddToCombatPairs( List<CombatPair> newPairs)
    {
        Data.CombatPairs.AddRange(newPairs);
    }

    void RemoveAlteredCombatPairs()
    {
        foreach (var alteredTower in Data.AlteredTowers)
        {
            Data.CombatPairs.RemoveAll(pair => pair.Contains(alteredTower));
        }
    }
    
    void Fire()
    {
        Data.CombatPairs.ForEach(p=>p.Shoot());
    }
    
    public override void Unsubscribe()
    {
        Eventbus.FireEvents.OnPairsAltered -= AddToCombatPairs;
    }
    
    // foreach (var alteredTower in Data.AlteredTowers)
    // {
    //     foreach (var pair in Data.CombatPairs)
    //     {
    //         if (pair.Contains(alteredTower))
    //         {
    //             Data.CombatPairs.Remove(pair);
    //         }
    //     }
    // }
   

    // void RestoreAlteredCombatPairs()
    // {
    //     foreach (var tower in Data.AlteredTowers)
    //     {
    //         foreach (var combatPair in tower.Data.CombatPairs)
    //         {
    //             Data.CombatPairs.Remove(combatPair);
    //             tower.SetCombatPairs();
    //             Data.CombatPairs.Add(combatPair);
    //         }
    //         //Data.CombatPairs.RemoveAll(cp => tower.Data.CombatPairs.Contains(cp));
    //     }
    // }

    // void ReorderAlteredTowers() //buna gerek yok ya, pairi çıkarıcaz zaten.
    // {
    //     for (int i = Data.CombatPairs.Count - 1; i >= 0; i--)
    //     {
    //         foreach (var tower in Data.AlteredTowers)
    //         {
    //             if (!Data.CombatPairs[i].HasTower(tower)) continue;
    //             
    //             Data.CombatPairs.Remove(Data.CombatPairs[i]);
    //             break;
    //         }
    //     }
    // }

   
}
