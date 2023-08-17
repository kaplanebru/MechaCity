using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class FireData : BaseTurnData
{
    public List<CombatPair> CombatPairs = new();
    public List<PassivePair> PassivePairs = new();

    public List<Tower> AlteredTowers = new();
}

public class FireHandler : BaseTurnHandler, ITurnActionHandler<FireData>
{
    public FireData Data { get; private set; }

    public override void Subscribe()
    {
        Data = new(); //Startta yapılabilir
        //Data.CombatPairs.Clear(); //sadece değişenlerin reoder edildiği dinamik bir sistem yapılabilir

        Eventbus.FireEvents.OnPairsAltered += AddToFightingPairsList;
        Eventbus.FireEvents.OnEvenPairs += AddToPassivePairsList;
        //Eventbus.FireEvents.OnFireEnabled.Invoke();
        
        Fire();
    }

    

    public override void ProcessTransferredData(BaseTurnData data) //(params object[] args)
    {
        var incomingData = (TowerGroupData)data;
        Data.AlteredTowers = incomingData.TowerGroup;
    }

    private void AddToFightingPairsList(CombatPair newPair)
    {
        Data.CombatPairs.Add(newPair);
    }
    
    private void AddToPassivePairsList(PassivePair evenPair)
    {
        Data.PassivePairs.Add(evenPair);
    }

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
    //             if (!Data.CombatPairs[i].FindTower(tower)) continue;
    //             
    //             Data.CombatPairs.Remove(Data.CombatPairs[i]);
    //             break;
    //         }
    //     }
    // }

    void Fire()
    {
        Data.CombatPairs.ForEach(p=>p.Shoot());
    }
    
    public override void Unsubscribe()
    {
        Eventbus.FireEvents.OnPairsAltered -= AddToFightingPairsList;
        Eventbus.FireEvents.OnEvenPairs -= AddToPassivePairsList;
    }
}
