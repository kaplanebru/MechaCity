using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;using Datas;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MatchData : BaseTurnData //bizim sonrakine göndereceğimiz
{
    //yeni matchleri gönderebilir. Gerçi bunlar zaten slotun ya da towerın kendisinde ekli.
}

public class MatchHandler : BaseTurnHandler, ITurnActionHandler<MatchData>
{
    public MatchData Data { get; private set; }
    public override void OnHandlerEnabled()
    {
        Data = new();
    }

    public override void Setup()
    {
    }

    public List<Tower> alteredTowers = new();

    public override void ProcessTransferredData(BaseTurnData data) //(params object[] args)
    {
        var incomingData = (TowerGroupData)data;
        alteredTowers = incomingData.TowerGroup;
    }
    
    // void MatchTowers(int number1, int number2)
    // {
    //     //currentPlayer.Data.Grid.Slots[number1].Tower.Data.LinkedTowers.Add(rivalPlayer.Data.Grid.Slots[number2].Tower);
    //     
    // }

    void LinkTowers(Tower tower1, Tower tower2)
    {
        if(!tower1.Data.LinkedTowers.Contains(tower2))
            tower1.Data.LinkedTowers.Add(tower2);
    }

    void CheckSlots(int number, Slot slot, GameGrid otherGrid)
    {
        if (number < 0 || number == GameGrid.SlotAmount) return;
        var rivalSlot = otherGrid.Slots[number];
        if (rivalSlot.hasTower)
        {
            LinkTowers(slot.Tower, rivalSlot.Tower);
        }
        else
        {
            if (otherGrid.Slots[number - 1].hasTower || otherGrid.Slots[number + 1].hasTower)
            {
                //LinkTowers();
                //counter: eğer tek eşleme yapacak olsaydık counter>0 olunca break derdik.
            }
                
        }
            
    }

    public override void Unsubscribe() {}

}

public interface IMatchable<out TTeamData>
{
    public TTeamData TeamData { get; }
    public Dictionary<int, int> Matches { get; set; }

  
    // void MatchSlots(GameGrid otherGrid, int slotNumber, Slot slot)
    // {
    //     if (slotNumber < 0 || slotNumber == GameGrid.SlotAmount) return;
    //     if (otherGrid.Slots[slotNumber].hasTower)
    //         slot.rivalSlotNumber = slotNumber;
    //     else
    //     {
    //         MatchSlots(otherGrid, slotNumber-1, slot);
    //         MatchSlots(otherGrid, slotNumber+1, slot);
    //     }
    // }


    void UpdateLinkedTowers(Slot slot, int deadNumber, GameGrid otherGrid)
    {
        //if (!slot.hasTower) return;
        
        for (int i = 0; i < GameGrid.SlotAmount-1; i++)
        {
            int counter = 0;
            int number = deadNumber - i;
            if (number >= 0)
            {
                if (otherGrid.Slots[number].hasTower)
                {
                    Match(slot.Number, number);
                    counter++;
                }
                    
            }

            number = deadNumber + i;
            if (number < GameGrid.SlotAmount)
            {
                if (otherGrid.Slots[number].hasTower)
                {
                    Match(slot.Number, number);
                    counter++;
                }
            }
            
            if(counter>0) break;
        }
    }


    
    void Match(int number1, int number2)
    {
    }
    
}