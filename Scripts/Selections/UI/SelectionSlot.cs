using System.Collections;
using System.Collections.Generic;
using Actor;
using Teams;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSlot : MonoBehaviour
{
    public int Index { get; private set; }

    public uint actorID;
    [SerializeField]private TextMeshProUGUI text;
    [SerializeField]private Image image;
    [SerializeField]private Color[] colors;

    public void ResetSlot()
    {
        text.text = "";
        actorID = 0; //-1
    }

    public void Fill(uint actorId)
    {
        text.text = SetText(actorId);
        actorID = actorId;
    }

    string SetText(uint actorId)
    {
        string towerText = "";
        var actor = ActorDB.Registry[actorId];
        for (var i = 0; i < actor.TowerIDs.Length; i++)
        {
            var towerID = actor.TowerIDs[i];
            if (i == 0)
            {
                towerText = RomanNumberConverter.IntToRoman(towerID + 1);
            }
            else
            {
                towerText = towerText + " - " + RomanNumberConverter.IntToRoman(towerID + 1);
            }
        }

        return towerText;
    }

    public void SetTeamColor(Team team)
    {
        image.color = team.Data.teamColorData.TeamColor; //temp. Uİ Color da ekli olabilir belki
    }

    public void SetIndex(int index)
    {
        Index = index;
    }
    
}
