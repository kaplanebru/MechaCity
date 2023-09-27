using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Models;
using Unity.Collections;
using UnityEngine;

public class CombatTransferData : BaseTurTransferData // = sıfırlanacak data
{
    public List<Tower> AlteredTowers = new();
    public List<TowerGridRelationModel> DeadTowers = new();
}

public class CombatData
{
    public List<CombatPair> CombatPairs = new();
    [ReadOnly] public float projectileSpeed = 1;
    public float ProjectileSpeed => projectileSpeed;
}

public class CombatHandler : BaseTurnHandler, ITurnActionHandler<CombatTransferData>
{
    public CombatTransferData TransferData { get; private set; }
    public override TurnHandlerType HandlerType => TurnHandlerType.Combat;
    private readonly CombatData Data = new();
    private float _combatSpeed;
    private Tower deadTower;


    public override void OnHandlerEnabled()
    {
        TransferData = new(); //bug: sıfırlanmış oluyor, eski tower listesi uçuyor. Transfer TransferData ve normal TransferData diye ayırmak gerekebilir
        Eventbus.FireEvents.OnTowerGridDetection += LatestDeadTower;
        Eventbus.FireEvents.OnFireEnabled?.Invoke();
    }

    private void LatestDeadTower(TowerGridRelationModel obj)
    {
        deadTower = obj.Tower;
        Data.CombatPairs.RemoveAll(p => p.Contains(deadTower));
    }

    // private void LatestDeadTower(Tower obj)
    // {
    //     deadTower = obj;
    //     Data.CombatPairs.RemoveAll(p => p.Contains(deadTower));
    // }

    public override void ProcessIncomingData(BaseTurTransferData data)
    {
        var incomingData = (TowerGroupData) data;
        TransferData.AlteredTowers = incomingData.TowerGroup; //bug: sıfırlanmış oluyor, eski tower listesi uçuyor
    }

    public override void Setup()
    {
        RemoveAlteredCombatPairs();
        //++ RemoveDeadPairs
        TransferData.AlteredTowers.ForEach(CreateCombatPairByTower);

        StartCoroutine(nameof(FireRoutine));
    }
    

   
    IEnumerator FireRoutine()
    {
        Data.CombatPairs = Data.CombatPairs.OrderBy(p => p.Perpetrator.Data.SlotId).ToList();

        int j = 0;  //önceki pairler de gidiyor ve mevcut j gerilemiş oluyor
        while (true)
        {
            var pair = Data.CombatPairs[j];
            _combatSpeed = pair.IsEven ? 0.1f : Data.ProjectileSpeed;
            
            pair.Combat(_combatSpeed);
            
           
            yield return new WaitForSeconds(_combatSpeed + 0.5f);

            j = pair.Perpetrator.Data.SlotId; //mevcut pair de siliniyor aslında, o yüzden yerine geçiyor
            print(j + " pairs: " + Data.CombatPairs.Count);
            //j++;
            if(j >= Data.CombatPairs.Count)
                break;
        }
        
        // for (var j = Data.CombatPairs.Count - 1; j >= 0; j--)
        // {
        //     var pair = Data.CombatPairs[j];
        //     if(pair == null) continue;
        //     _combatSpeed = pair.IsEven ? 0.1f : Data.ProjectileSpeed;
        //
        //     pair.Combat(_combatSpeed);
        //     yield return new WaitForSeconds(_combatSpeed + 0.5f);
        //     
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

    void AddToPairs(Tower tower1, Tower tower2,
        bool isEven = false) //TODO: bunun yerine slot id'ye göre insert yapabiliriz
    {
        Data.CombatPairs.Add(new CombatPair(tower1, tower2, isEven));
        if (!isEven)
            tower1.Data.BulletAmount--;
    }

    void RemoveAlteredCombatPairs()
    {
        foreach (var alteredTower in TransferData.AlteredTowers)
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
        TransferData.AlteredTowers.ForEach(t => t.towerParts.SetColor(t.Data.TeamTowerData.DefaultMaterial));
    }


    public override void Unsubscribe()
    {
        DeselectAlteredTowers();
        Eventbus.FireEvents.OnTowerGridDetection -= LatestDeadTower;
    }
}