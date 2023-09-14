using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Models;
using Unity.Collections;
using UnityEngine;

public class CombatData : BaseTurnData
{
    public List<CombatPair> CombatPairs = new();
    public List<Tower> AlteredTowers = new();
    public List<TowerGridRelationModel> DeadTowers = new();
    
    [ReadOnly] public float projectileSpeed = 1;
    public float ProjectileSpeed => projectileSpeed;
    public float FireSpeedMultiplier = 0.7f;
}

public class CombatHandler : BaseTurnHandler, ITurnActionHandler<CombatData>
{
    public CombatData Data { get; private set; }
    public override TurnHandlerType HandlerType => TurnHandlerType.Combat;

    public override void OnHandlerEnabled()
    {
        Data = new();
        Data.DeadTowers.Clear();
        Eventbus.FireEvents.OnTowerTeamDetection += AddToDeadTowers;
        Eventbus.FireEvents.OnFireEnabled?.Invoke();
        
    }
    
    public override void ProcessIncomingData(BaseTurnData data) 
    {
        var incomingData = (TowerGroupData) data;
        Data.AlteredTowers = incomingData.TowerGroup;
    }
    public override void Setup()
    {
         RemoveAlteredCombatPairs();
         Data.AlteredTowers.ForEach(CreateCombatPairByHeight);
         
         StartCoroutine(nameof(FireRoutine));
    }
    
    
    private void AddToDeadTowers(TowerGridRelationModel towerGridRelationModel)
    {
        Data.DeadTowers.Add(towerGridRelationModel);
    }
    
    IEnumerator FireRoutine()
    {
        foreach (var pair in Data.CombatPairs)
        {
            pair.Combat(Data.ProjectileSpeed);
            yield return new WaitForSeconds(Data.ProjectileSpeed); // * Data.FireSpeedMultiplier);
        }
        
        yield return new WaitForSeconds(0.1f);
        CompleteAction();
    }

    void CreateCombatPairByHeight(Tower tower)
    {
        OrderLinkedTowersByDistance(tower);

        foreach (var linkedTower in tower.Data.LinkedTowers)
        {
            if (tower.Data.Height > linkedTower.Data.Height)
            {
                if(!tower.Data.CanShoot) continue;
                AddToPairs(tower, linkedTower);
            }
            else if (linkedTower.Data.Height > tower.Data.Height)
            {
                if(!linkedTower.Data.CanShoot) continue;
                AddToPairs(linkedTower, tower);
            }
            else
                AddToPairs(linkedTower, tower, true);
        }
    }

    void AddToPairs(Tower tower1, Tower tower2, bool isEven=false)
    {
        Data.CombatPairs.Add(new CombatPair(tower1, tower2, isEven));
        if (!isEven) 
            tower1.Data.BulletAmount--;
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
        tower.Data.LinkedTowers =
            tower.Data.LinkedTowers.OrderBy(other => Mathf.Abs(tower.Data.SlotId - other.Data.SlotId)).ToList();
    }

    void DeselectAlteredTowers() //TODO: At the end of animation
    {
        Data.AlteredTowers.ForEach(t=> t.towerParts.SetColor(t.Data.TeamTowerData.DefaultMaterial));
    }
    

    public override void Unsubscribe()
    {
        DeselectAlteredTowers();
        Eventbus.FireEvents.OnTowerTeamDetection -= AddToDeadTowers;
    }
}