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
        public BaseInfoText playerText;
        public TurnButtonsHandler turnButtonsHandler;
        private void OnEnable()
        {
            turnButtonsHandler = GetComponentInChildren<TurnButtonsHandler>();
            NetworkEventbus.OnAllClientsSet += ShowInfoText; //todo: clients set aynı zamanda bug sebebi çünkü first turn böyle başlıyor
            UIEventbus.OnPlayerSet += ShowPlayerText;
            
            
            NetworkEventbus.UIEvents.OnTurnButtonShiftRequest += EnableTurnButton;
            DisableUIs();
        }
        private void EnableTurnButton(bool enable)
        {
            Debug.Log("enable turn button: " + enable);
            turnButtonsHandler.gameObject.SetActive(enable);
        }

        void DisableUIs()
        {
            playerText.gameObject.SetActive(false);
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
        
        private void ShowPlayerText(string name)
        {
           playerText.gameObject.SetActive(true);
           playerText.SetInfoText(name);
        }
    
        private void OnDisable()
        {
            NetworkEventbus.OnAllClientsSet -= ShowInfoText;
            UIEventbus.OnPlayerSet -= ShowPlayerText;
            NetworkEventbus.UIEvents.OnTurnButtonShiftRequest -= EnableTurnButton;
    
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

