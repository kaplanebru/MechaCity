using System.Collections;
using System.Collections.Generic;
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

    public void Fill(string towerName, uint id)
    {
        text.text = towerName;
        actorID = id;
    }

    public void SetTeamColor(Team team)
    {
        image.color = team.Data.TeamTowerData.TeamColors[0]; //temp. Uİ Color da ekli olabilir belki
    }

    public void SetIndex(int index)
    {
        Index = index;
    }
    
}
