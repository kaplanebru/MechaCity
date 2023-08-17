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
        Data.AlteredTowers.ForEach(CreateCombatPairsByHeight);
        //Eventbus.FireEvents.OnPairsAltered += AddToCombatPairs;

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

    private void SelectCombatPair(CombatPair newPair)
    {
        if(newPair.Perpetrator.bulletAmount>0)
            Data.CombatPairs.Add(newPair);
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
    
    void CreateCombatPairsByHeight(Tower tower)
    {
        foreach (var other in tower.Data.LinkedTowers)
        {
            if (tower.Data.Height > other.Data.Height)
                SelectCombatPair(new CombatPair(tower, other));
            else if(tower.Data.Height < other.Data.Height)
                SelectCombatPair(new CombatPair(other, tower));
            else
                SelectCombatPair(new CombatPair(other, tower, true));
        }
    }
}
