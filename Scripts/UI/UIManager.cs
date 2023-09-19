using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button playButton;

    private void OnEnable()
    {
        playButton.gameObject.SetActive(false);
        Eventbus.NetworkEvents.OnAllClientsSet += ShowPlayButton;
    }

    private void ShowPlayButton()
    {
        playButton.gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        Eventbus.NetworkEvents.OnAllClientsSet -= ShowPlayButton;
    }
}
