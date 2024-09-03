using System;
using System.Collections;
using System.Collections.Generic;
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
        
        [SerializeField] private Button button;
        [SerializeField] private Image buttonImage;
        
        [SerializeField] private Color[] buttonColors;
        [SerializeField] private TextMeshProUGUI buttonTextSlot;
        public TurnButtonHolder buttonHolder;

        private TurnStateType _currentType = TurnStateType.Exit;
        private TurnStateType _oldType = TurnStateType.Exit;

        private void OnEnable() //ui daha önce gelmeli turnden
        {
            Subscribe();
            buttonTextSlot.fontSizeMax = 20;
        }
        
        void Subscribe()
        {
            Highlight(false);
            UIEventbus.OnShowButtonRequest += ShowButton;
        }

        void Highlight(bool shine)
        {
            buttonImage.color = shine ? buttonColors[1] : buttonColors[0];
        }
        
        void ShowButton(bool enable, TurnStateType type)
        {
            _oldType = _currentType;
            _currentType = type;
            
            if(type != _oldType)
                SetButton(type);
            Highlight(enable);
        }

        void SetButton(TurnStateType type)
        {
            // buttonTextSlot.text = buttonHolder.ButtonsByType[type].Content;
            // buttonTextSlot.fontSizeMax = 40;
            // buttonTextSlot.enableAutoSizing = true;
            //TODO: tooltip olarak çıkabilir
        }

        public void ButtonClicked()
        {
            UIEventbus.OnButtonClicked?.Invoke();
            Highlight(false);
        }
        
        private void OnDisable()
        {
            UIEventbus.OnShowButtonRequest -= ShowButton;
        }
    }
}