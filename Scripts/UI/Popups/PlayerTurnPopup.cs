using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;

public class PlayerTurnPopup : MonoBehaviour
{
    public BaseInfoText popupText;
    private void OnEnable()
    {
        Eventbus.UIEvents.OnTeamSwitch += ShowPopup;
    }

    private void ShowPopup(TeamType currentTeamType)
    {
        popupText.gameObject.SetActive(true);
        popupText.UpdateInfoText(currentTeamType);
    }
    

    private void OnDisable()
    {
        Eventbus.UIEvents.OnTeamSwitch -= ShowPopup;
    }
}
