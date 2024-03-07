using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using Enums;
using Network;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class TurnButtonsHandler : MonoBehaviour
    {
        
        [SerializeField] private Button button;
        public TurnButtonHolder buttonHolder;

        private void OnEnable() //ui daha önce gelmeli turnden
        {
            Subscribe();
        }
        
        void Subscribe()
        {
            DisableAllButtons();
            UIEventbus.OnShowButtonRequest += ShowButton;
        }
        
        void ShowButton(bool enable, TurnStateType type)
        {
            print("show button on type: " + type);
            button.gameObject.SetActive(enable);
        }
        

        void DisableAllButtons()
        {
            button.gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            UIEventbus.OnShowButtonRequest -= ShowButton;
        }
    }
}