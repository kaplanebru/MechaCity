using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class GameGrid  //TODO: SO yapılabilir : 2 tane türetirilir
{
    [ReadOnly]public TeamType TeamType;
    public const int SlotAmount = 3;
    [ReadOnly]public Slot[] Slots = new Slot[SlotAmount];

    public void Initialize(Team team)
    {
        Setup(team);
        Eventbus.FireEvents.OnTowerKilled += SendGridInfo;
    }

    void Setup(Team team)
    {
        TeamType = team.Data.teamCosmeticData.teamType;
        CreateSlots();
        SetSlots(team);
    }

    private void SendGridInfo(Tower deadTower)
    {
        if(deadTower.Data.teamCosmeticData.teamType != TeamType) return;
        Eventbus.FireEvents.OnTowerDied?.Invoke(new TowerGridRelationModel(this, deadTower));
    }

    void CreateSlots()
    {
        for (int i = 0; i < SlotAmount; i++)
        {
            Slots[i] = new Slot
            {
                HasTower = true
            };
        }
    }
    
    void SetSlots(Team team)
    {
        for (int i = 0; i < SlotAmount; i++)
        {
            Slots[i].Id = i;
            Slots[i].Tower = team.Data.Towers[i];
        }
    }

    public void DisableGrid()
    {
        Eventbus.FireEvents.OnTowerKilled -= SendGridInfo;
    }


}
