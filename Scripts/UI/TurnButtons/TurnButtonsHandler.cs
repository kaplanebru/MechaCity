using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Enums;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class TurnButtonsHandler : MonoBehaviour
    {
        [SerializeField] private TurnButton[] turnButtons;
        [SerializeField] private GameObject buttonsHolder;
        private TurnButton _currentButton;
        private bool isActive = true;

        private void OnEnable() //ui daha önce gelmeli turnden
        {
            DisableAllButtons();
        }

     

        public void SubscribeAndOpenButtons()
        {
            buttonsHolder.SetActive(true);
            UIEventbus.OnHighlightRequest += Highlight;
            UIEventbus.OnStateShift += ShiftButtonState;
        }

        private void Highlight(bool enable)
        {
            _currentButton.Highlight(enable);
        }


        void ShiftButtonState(TurnStateType stateType)
        {
            DisableAllButtons();

            if (stateType == TurnStateType.Combat)//(type != TurnStateType.Selection && type != TurnStateType.Link && type != TurnStateType.Intruder)
            {
                UnsubscribeAndCloseButtonHolder();
                return;
            }

            _currentButton = turnButtons.FirstOrDefault(b => b.turnStateType == stateType);

            if (!_currentButton) return;
            
            _currentButton.Highlight(false);
            _currentButton.gameObject.SetActive(true);
        }

        public void UnsubscribeAndCloseButtonHolder()
        {
            buttonsHolder.SetActive(false);
            Unsubscribe();
        }

        public void ButtonClicked()
        {
            UIEventbus.OnButtonClicked?.Invoke();
        }

        void DisableAllButtons()
        {
            foreach (var turnButton in turnButtons)
            {
                turnButton.gameObject.SetActive(false);
            }
        }

        void Unsubscribe()
        {
            UIEventbus.OnHighlightRequest -= Highlight;
            UIEventbus.OnStateShift -= ShiftButtonState;
        }
    }
}