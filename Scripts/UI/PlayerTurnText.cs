using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using TMPro;
using UnityEngine;

public class PlayerTurnText : MonoBehaviour
{
    private Team[] _teams;
    TextMeshProUGUI infoText;
    
    private void OnEnable()
    {
        infoText = GetComponentInChildren<TextMeshProUGUI>();
        Eventbus.NetworkRequestEvents.TeamSwitchRequest += UpdateInfoText;
    }

    public void Setup(Team[] teams)
    {
        _teams = teams;
        SetInfoText(_teams[0].Data.Name);
    }
    void UpdateInfoText(TeamType currentTeamType)
    {
        SetInfoText(_teams.FirstOrDefault(t=>t.Data.TeamType == currentTeamType).Data.Name);
    }

    void SetInfoText(string teamName)
    {
        infoText.text = "Turn: " + teamName; 
    }

    private void OnDisable()
    {
        Eventbus.NetworkRequestEvents.TeamSwitchRequest -= UpdateInfoText;
    }
}
