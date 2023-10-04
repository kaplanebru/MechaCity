using System.Collections.Generic;
using Enums;
using Network;
using UnityEngine;

namespace GameUI
{
    public class UIManager : MonoBehaviour
    {
        //public Button playButton;
       
        public BaseInfoText turnInfoPopupText;
        public BaseInfoText turnInfoText;
        private void OnEnable()
        {
            NetworkEventbus.OnAllClientsSet += ShowInfoText;
            DisableUIs();
        }
    
        void DisableUIs()
        {
            turnInfoText.gameObject.SetActive(false);
            turnInfoPopupText.gameObject.SetActive(false);
        }
    
        private void ShowInfoText(params object[] args)
        {
            var teamNamesByType = args[0] as Dictionary<TeamType, string>;
            turnInfoText.gameObject.SetActive(true);
            turnInfoPopupText.gameObject.SetActive(true);
            turnInfoText.Setup(teamNamesByType);
            turnInfoPopupText.Setup(teamNamesByType);
        }
    
        private void OnDisable()
        {
            NetworkEventbus.OnAllClientsSet -= ShowInfoText;
    
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

