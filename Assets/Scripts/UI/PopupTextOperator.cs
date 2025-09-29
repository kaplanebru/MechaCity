using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class PopupTextOperator : MonoBehaviour
    {
        public PopupTextHolder textHolder;
        public GameObject popup;
        [SerializeField] private TextMeshProUGUI textSlot;

        private void OnEnable()
        {
            UIEventbus.OnPopupTime += ShowPopup;
        }

        private void OnDisable()
        {
            UIEventbus.OnPopupTime -= ShowPopup;
        }

        public async void ShowPopup(PopupType type)
        {
            var text = textHolder.popupByType[type];
            textSlot.text = text;
            popup.SetActive(true);

            await DelayMaker.WaitForSeconds(1);
            HidePopup();
        }

        public void HidePopup()
        {
            popup.SetActive(false);
        }
        
        //Current team, rival team çek
        //popup type, popup text dictionary
        //text içinde team adı geçiyorsa?
    }

}
