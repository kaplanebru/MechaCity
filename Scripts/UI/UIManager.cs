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
    public PlayerTurnText playerTurnText;
    private Team[] _teams;
    private void OnEnable()
    {
        Eventbus.NetworkEvents.OnAllClientsSet += ShowInfoText;
        playerTurnText.gameObject.SetActive(false);
    }

    private void ShowInfoText(Team[] teams)
    {
        playerTurnText.gameObject.SetActive(true);
        playerTurnText.Setup(teams);
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
