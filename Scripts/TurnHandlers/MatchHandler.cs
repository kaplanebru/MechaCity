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
    //public BasePlayer currentPlayer;
    public MatchData Data { get; private set; }
    public override void Subscribe()
    {
        Data = new();
    }

    public List<Tower> alteredTowers = new();

    public override void ProcessTransferredData(BaseTurnData data) //(params object[] args)
    {
        var incomingData = (TowerGroupData)data;
        alteredTowers = incomingData.TowerGroup;
    }

    void SetTargets()
    {
        for (int i = 0; i < alteredTowers.Count; i++)
        {
            //currentPlayer.Data.RivalData.Grid.Slots[alteredTowers[i].Data.Id]
        }
    }

    public override void Unsubscribe() {}

}

public interface IMatchable<out TTeamData>
{
    public TTeamData TeamData { get; }
    public Dictionary<int, int> Matches { get; set; }

  

    void SetTarget(Slot slot, GameGrid otherGrid)
    {
        for (int i = 0; i < GameGrid.SlotAmount; i++)
        {
            if(!slot.hasTower) continue;
            
            int number = slot.Number - i;
            if (number >= 0)
            {
                if (otherGrid.Slots[number].hasTower)
                {
                    Match(slot.Number, number);
                    break;
                }
            }

            number = slot.Number + i;
            if (number < GameGrid.SlotAmount)
            {
                if (otherGrid.Slots[number].hasTower)
                {
                    Match(slot.Number, number);
                    break;
                }
            }
        }
    }


    
    void Match(int number1, int number2)
    {
        
    }
    
}