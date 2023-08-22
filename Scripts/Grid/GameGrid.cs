using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class GameGrid  //TODO: SO yapılabilir : 2 tane türetirilir
{
    private Team TeamType;
    public const int SlotAmount = 3;
    [ReadOnly]public Slot[] Slots = new Slot[SlotAmount];

    public void Initialize(BasePlayer player)
    {
        Setup(player);
        Eventbus.FireEvents.OnTowerKilled += SendGrid;
    }

    void Setup(BasePlayer player)
    {
        TeamType = player.Data.TeamData.Team;
        CreateSlots();
        SetSlots(player);
    }

    private void SendGrid(Tower deadTower)
    {
        if(deadTower.Data.TeamType != TeamType) return;
        Eventbus.FireEvents.OnTowerDied?.Invoke(new TowerGridRelationModel(this, deadTower));
    }

    void CreateSlots()
    {
        for (int i = 0; i < SlotAmount; i++)
        {
            Slots[i] = new Slot();
        }
    }
    
    void SetSlots(BasePlayer player)
    {
        for (int i = 0; i < SlotAmount; i++)
        {
            Slots[i].Number = i;
            Slots[i].Tower = player.Data.Towers[i];
        }
    }

    public void DisableGrid()
    {
        Eventbus.FireEvents.OnTowerKilled -= SendGrid;
    }


}
