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
        Eventbus.FireEvents.OnFireEnabled?.Invoke();

        RemoveAlteredCombatPairs();
        Data.AlteredTowers.ForEach(CreateCombatPairByHeight);

        Fire();
    }

    public override void ProcessTransferredData(BaseTurnData data)
    {
        var incomingData = (TowerGroupData) data;
        Data.AlteredTowers = incomingData.TowerGroup;
    }

    void Fire()
    {
        Data.CombatPairs.ForEach(p => p.Combat());

        //bullet anim.OnComplete:
        //Heighte göre Dotween eklenir
    }

    void CreateCombatPairByHeight(Tower tower)
    {
        OrderLinkedTowersByDistance(tower);

        foreach (var other in tower.Data.LinkedTowers)
        {
            if (tower.Data.Height > other.Data.Height)
            {
                if(tower.bulletAmount == 0) continue;
                Data.CombatPairs.Add(new CombatPair(tower, other));
                tower.bulletAmount--;
            }
            else if (other.Data.Height > tower.Data.Height)
            {
                if(other.bulletAmount == 0) continue;
                Data.CombatPairs.Add(new CombatPair(other, tower));
                other.bulletAmount--;
            }
            else
                Data.CombatPairs.Add(new CombatPair(other, tower, true));
        }
    }
    

    void OrderLinkedTowersByDistance(Tower tower)
    {
        //slot id'ye göre de dizilebilir.
        tower.Data.LinkedTowers =
            tower.Data.LinkedTowers.OrderBy(other => Mathf.Abs(tower.Data.Id - other.Data.Id)).ToList();
    }
    
    void RemoveAlteredCombatPairs()
    {
        foreach (var alteredTower in Data.AlteredTowers)
        {
            Data.CombatPairs.RemoveAll(pair => pair.Contains(alteredTower));
        }
    }

    public override void Unsubscribe()
    {
    }
}