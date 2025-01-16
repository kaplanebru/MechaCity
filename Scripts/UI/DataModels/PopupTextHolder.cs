using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace GameUI
{
    [CreateAssetMenu(menuName = "UI/ " + nameof(PopupTextHolder), fileName = nameof(PopupTextHolder))]
    public class PopupTextHolder : ScriptableObject
    {
        public Dictionary<PopupType, string> popupByType = new();
        [SerializeField] private TypeDataCouple<PopupType, string>[] serializedPopupByType;

        public void Setup()
        {
            foreach (var couple in serializedPopupByType)
            {
                popupByType.Add(couple.Type, couple.Data);
            }
        }
        private void OnEnable()
        {
            Setup();
        }
    }

}
