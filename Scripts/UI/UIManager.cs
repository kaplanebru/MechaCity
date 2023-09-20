using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //public Button playButton;
    public Transform turnInfoParent;
    public TextMeshProUGUI turnInfo;
    private Team[] _teams;
    private void OnEnable()
    {
        turnInfoParent.gameObject.SetActive(false);
        Eventbus.NetworkEvents.OnAllClientsSet += ShowInfoText;
        Eventbus.NetworkRequestEvents.TeamSwitchRequest += UpdateInfoText;
    }

    private void ShowInfoText(Team[] teams)
    {
        turnInfoParent.gameObject.SetActive(true);
        _teams = teams;
        SetInfoText(_teams[0].Data.Name); 
    }

    void UpdateInfoText(TeamType currentTeamType)
    {
        SetInfoText(_teams.FirstOrDefault(t=>t.Data.TeamType == currentTeamType).Data.Name);
    }

    void SetInfoText(string teamName)
    {
        turnInfo.text = "Turn: " + teamName; 
    }

    private void OnDisable()
    {
        Eventbus.NetworkEvents.OnAllClientsSet -= ShowInfoText;
    }

    #region Play Button

    // private void OnEnable()
    // {
    //     playButton.gameObject.SetActive(false);
    //     Eventbus.NetworkEvents.OnAllClientsSet += ShowPlayButton;
    // }
    //
    // private void ShowPlayButton()
    // {
    //     playButton.gameObject.SetActive(true);
    // }
    //
    //
    // private void OnDisable()
    // {
    //     Eventbus.NetworkEvents.OnAllClientsSet -= ShowPlayButton;
    // }

    #endregion

}
