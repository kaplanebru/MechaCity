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
        private TurnButton _currentButton;
        
        private void OnEnable() //ui daha önce gelmeli turnden
        {
            DisableAll();
            Subscribe();
            //DisableAll();
            //buttonTextSlot.fontSizeMax = 20;
        }
        
        void Subscribe()
        {
            UIEventbus.OnStateShift += ShiftButton;
            UIEventbus.OnHighlightRequest += Highlight;
            Debug.Log("subscribe to turn buttons");
        }

        private void Highlight(bool enable)
        {
            _currentButton.Highlight(enable);
        }


        void ShiftButton(TurnStateType type)
        {
            DisableAll();
            
            _currentButton = turnButtons.FirstOrDefault(b=>b.turnStateType == type);
            if (!_currentButton)
                return;

            _currentButton.Highlight(false);
            _currentButton.gameObject.SetActive(true);
        }

        public void ButtonClicked()
        {
            UIEventbus.OnButtonClicked?.Invoke();
        }

        void DisableAll()
        {
            foreach (var turnButton in turnButtons)
            {
                turnButton.gameObject.SetActive(false);
            }
        }
        
        private void OnDisable()
        {
            UIEventbus.OnStateShift -= ShiftButton;
            UIEventbus.OnHighlightRequest -= Highlight;
            Debug.Log("unsubscribe from turn buttons");

        }
    }
}