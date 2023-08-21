using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

public class FireData : BaseTurnData
{
    public List<CombatPair> CombatPairs = new();
    public List<Tower> AlteredTowers = new();
    public List<Tower> DeadTowers = new();
}

public class CombatPairHandler : BaseTurnHandler, ITurnActionHandler<FireData>
{
    public FireData Data { get; private set; }

    public override void OnHandlerEnabled()
    {
        Data = new();
        Data.DeadTowers.Clear();
        Eventbus.FireEvents.OnTowerDied += AddToDeadTowers;
        Eventbus.FireEvents.OnFireEnabled?.Invoke();
    }

    
    //SO yapıp elle gönderebilir miyiz? hayır, değişken datalar. SOdaki verileri input olarak alıyoruz, outputu etkilemiyor
    //SO ortak bir input yani.
    //Mesela Grid bilgisi So olarak yollanabilir!!!!
    public override void ProcessTransferredData(BaseTurnData data) 
    {
        var incomingData = (TowerGroupData) data;
        Data.AlteredTowers = incomingData.TowerGroup;
    }
    public override void Setup()
    {
         RemoveAlteredCombatPairs();
         Data.AlteredTowers.ForEach(CreateCombatPairByHeight);
        
         Fire();
    }
    
    
    private void AddToDeadTowers(Tower deadTower)
    {
        //deadTower.Data.TeamData.Team ==

        Data.DeadTowers.Add(deadTower);
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
                if(!tower.CanShoot) continue;
                Data.CombatPairs.Add(new CombatPair(tower, other));
                tower.BulletAmount--;
            }
            else if (other.Data.Height > tower.Data.Height)
            {
                if(!other.CanShoot) continue;
                Data.CombatPairs.Add(new CombatPair(other, tower));
                other.BulletAmount--;
            }
            else
                Data.CombatPairs.Add(new CombatPair(other, tower, true));
        }
    }
    
    void RemoveAlteredCombatPairs()
    {
        foreach (var alteredTower in Data.AlteredTowers)
        {
            Data.CombatPairs.RemoveAll(pair => pair.Contains(alteredTower));
        }
    }

    void OrderLinkedTowersByDistance(Tower tower)
    {
        //slot id'ye göre de dizilebilir.
        tower.Data.LinkedTowers =
            tower.Data.LinkedTowers.OrderBy(other => Mathf.Abs(tower.Data.Id - other.Data.Id)).ToList();
    }
    

    public override void Unsubscribe()
    {
        Eventbus.FireEvents.OnTowerDied -= AddToDeadTowers;
    }
}