using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using TMPro;
using UnityEngine;
using Teams;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        //public Button playButton;
        public BaseInfoText turnInfoPopupText;
        public BaseInfoText turnInfoText;
        private Team[] _teams;
        private void OnEnable()
        {
            Eventbus.NetworkEvents.OnAllClientsSet += ShowInfoText;
            DisableUIs();
        }
    
        void DisableUIs()
        {
            turnInfoText.gameObject.SetActive(false);
            turnInfoPopupText.gameObject.SetActive(false);
        }
    
        private void ShowInfoText(Team[] teams)
        {
            turnInfoText.gameObject.SetActive(true);
            turnInfoPopupText.gameObject.SetActive(true);
            turnInfoText.Setup(teams);
            turnInfoPopupText.Setup(teams);
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

}

