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
        Data = new(); //bug: sıfırlanmış oluyor, eski tower listesi uçuyor. Transfer Data ve normal Data diye ayırmak gerekebilir
        Data.DeadTowers.Clear();
        Eventbus.FireEvents.OnTowerKilled += LatestDeadTower;
        //Eventbus.FireEvents.OnTowerGridDetection += AddToDeadTowers;
        Eventbus.FireEvents.OnFireEnabled?.Invoke();
    }

    private Tower deadTower;
    private void LatestDeadTower(Tower obj)
    {
        deadTower = obj;
    }

    public override void ProcessIncomingData(BaseTurnData data)
    {
        var incomingData = (TowerGroupData) data;
        Data.AlteredTowers = incomingData.TowerGroup;  //bug: sıfırlanmış oluyor, eski tower listesi uçuyor
    }

    public override void Setup()
    {
        RemoveAlteredCombatPairs();
        Data.AlteredTowers.ForEach(CreateCombatPairByTower);

        StartCoroutine(nameof(FireRoutine));
    }


    private void AddToDeadTowers(TowerGridRelationModel towerGridRelationModel)
    {
        Data.DeadTowers.Add(towerGridRelationModel);
    }

    private float _combatSpeed;
    IEnumerator FireRoutine()
    {
        print(teams["currentTeam"].Data.Towers.Count);
        for (int i = 0; i < teams["currentTeam"].Data.Towers.Count; i++) //teamden çıkan olabiliyor o yüzden sürekli check
        {
            Data.CombatPairs.Clear();
            CreateCombatPairByTower(teams["currentTeam"].Data.Towers[i]);
            
            
            
            foreach (var pair in Data.CombatPairs)
            {
                
                _combatSpeed = pair.IsEven ? 0.1f : Data.ProjectileSpeed;
            
                pair.Combat(_combatSpeed);
                yield return new WaitForSeconds(_combatSpeed); // * Data.FireSpeedMultiplier);
                //Data.CombatPairs.RemoveAt(0); //test
                //yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(1);
           
        }


        // foreach (var pair in Data.CombatPairs)
        // {
        //     _combatSpeed = pair.IsEven ? 0.1f : Data.ProjectileSpeed;
        //
        //     pair.Combat(_combatSpeed);
        //     yield return new WaitForSeconds(_combatSpeed); // * Data.FireSpeedMultiplier);
        //     //Data.CombatPairs.RemoveAt(0); //test
        // }
        
        

        // for (int i = Data.CombatPairs.Count - 1; i >= 0; i--)
        // {
        //     var pair = Data.CombatPairs[i];
        //     _combatSpeed = pair.IsEven ? 0.1f : Data.ProjectileSpeed;
        //
        //     //dead tower rematch olduktan sonra yapılmalı, rematchsiz haliyle eski linklere göre çalışır
        //     if (pair.Contains(deadTower))
        //     {
        //         Data.CombatPairs.Remove(pair);
        //         CreateCombatPairByTower(deadTower);
        //         continue;
        //     }
        //     
        //     //on death: remove related pairs, add new pairs
        //     pair.Combat(_combatSpeed);
        //     yield return new WaitForSeconds(_combatSpeed); 
        // }

        yield return new WaitForSeconds(0.1f);
        CompleteAction();
    }

    void CreateCombatPairByTower(Tower tower)
    {
        OrderLinkedTowersByDistance(tower);

        for (var i = 0; i < tower.Data.LinkedTowers.Count; i++)
        {
            var linkedTower = tower.Data.LinkedTowers[i];
            if (tower.Data.Height > linkedTower.Data.Height)
            {
                if (!tower.Data.CanShoot) continue;
                AddToPairs(tower, linkedTower);
            }
            else if (linkedTower.Data.Height > tower.Data.Height)
            {
                if (!linkedTower.Data.CanShoot) continue;
                AddToPairs(linkedTower, tower);
            }
            else
                AddToPairs(linkedTower, tower, true);
        }
    }

    void AddToPairs(Tower tower1, Tower tower2, bool isEven = false) //TODO: bunun yerine slot id'ye göre insert yapabiliriz
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
        Data.AlteredTowers.ForEach(t => t.towerParts.SetColor(t.Data.TeamTowerData.DefaultMaterial));
    }


    public override void Unsubscribe()
    {
        DeselectAlteredTowers();
        //Eventbus.FireEvents.OnTowerGridDetection -= AddToDeadTowers;
        Eventbus.FireEvents.OnTowerKilled -= LatestDeadTower;

    }
}