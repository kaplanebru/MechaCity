using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;using Datas;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MatchData : BaseTurnData //bizim sonrakine göndereceğimiz
{
    public List<Tower> deadTowers = new();
    //yeni matchleri gönderebilir. Gerçi bunlar zaten slotun ya da towerın kendisinde ekli.
}

public class MatchHandler : BaseTurnHandler, ITurnActionHandler<MatchData>
{
    public MatchData Data { get; private set; }
    public override void OnHandlerEnabled()
    {
        Data = new();
    }

    public override void ProcessTransferredData(BaseTurnData data)
    {
        var incomingData = (FireData)data;
        Data.deadTowers = incomingData.DeadTowers;
    }
    
    public override void Setup()
    {
        //Data.deadTowers.ForEach();
        //TODO: dead tower'a linked olanları bul, bunların slotlarını bul, othergrid 2 taraftan da biri olabilir, yani dead tower kimlerden bilmemiz lazım
        
    }

    void LinkTowers(Tower tower1, Tower tower2)
    {
        if(!tower1.Data.LinkedTowers.Contains(tower2))
            tower1.Data.LinkedTowers.Add(tower2);
    }

    void RemoveLinkedTower(Tower tower1, Tower tower2)
    {
        tower1.Data.LinkedTowers.Remove(tower2);
    }

    void RestoreLinkedTowers(Slot slot, Tower deadTower, GameGrid otherGrid)
    {
        RemoveLinkedTower(slot.Tower, deadTower);
        int deadTowerId = deadTower.Data.Id;

        //if (!slot.hasTower) return;

        for (int i = 0; i < GameGrid.SlotAmount-1; i++)
        {
            int counter = 0;
            
            int number = deadTowerId - i;
            if (number >= 0) //&& number < GameGrid.SlotAmount
            {
                if (otherGrid.Slots[number].hasTower)
                {
                    LinkTowers(slot.Tower, otherGrid.Slots[number].Tower);
                    counter++;
                }
            }

            number = deadTowerId + i;
            if (number < GameGrid.SlotAmount)
            {
                if (otherGrid.Slots[number].hasTower)
                {
                    LinkTowers(slot.Tower, otherGrid.Slots[number].Tower);
                    counter++;
                }
            }
            
            if(counter>0) break;
        }
    }

    public override void Unsubscribe() {}

}