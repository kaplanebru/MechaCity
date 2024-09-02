using System.Collections;
using System.Collections.Generic;
using Teams;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSlot : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;
    [SerializeField]private Image image;
    [SerializeField]private Color[] colors;

    public void ResetName()
    {
        text.text = "";
    }

    public void SetName(string towerName)
    {
        text.text = towerName;
    }

    public void SetTeamColor(Team team)
    {
        image.color = team.Data.TeamTowerData.TeamColors[0]; //temp. Uİ Color da ekli olabilir belki
    }
    
}
