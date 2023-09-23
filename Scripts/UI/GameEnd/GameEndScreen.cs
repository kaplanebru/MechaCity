using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;

public class GameEndScreen : MonoBehaviour
{
    public Transform[] panels;

    private void OnEnable()
    {
        Eventbus.NetworkRequestEvents.OnGameEndScreenRequest += ShowPanel;
        DisableAll();
    }

    private void ShowPanel(GameEndState state)
    {
        switch (state)
        {
            case GameEndState.Win:
                panels[0].gameObject.SetActive(true);
                break;
            case GameEndState.Lose:
                panels[1].gameObject.SetActive(true);
                break;
        }
    }
    

    void DisableAll()
    {
        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Eventbus.NetworkRequestEvents.OnGameEndScreenRequest -= ShowPanel;

    }
}
