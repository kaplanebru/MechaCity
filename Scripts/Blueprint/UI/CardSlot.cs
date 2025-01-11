using System.Collections;
using System.Collections.Generic;
using DataModels;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blueprint
{
    public class CardSlot : MonoBehaviour
    {
        public BpType currentBpType;
        public BlueprintData Data;
        public int level = 1;
        public int amount = 3; //todo: test, send amount from player data
        
        public CardInteraction cardInteraction;
        public TextMeshPro titleHolder;
        public TextMeshPro priceHolder;
        public GameObject blocker;
        
        public virtual void Setup(BlueprintData data)
        {
            Data = data;
            SetTexts();
            Data.Level = level; //todo: check, ref type diye burdan yapılabilir diye düşündüm
            cardInteraction.Setup(data);
        }
        public void SetType(BpType type)
        {
            currentBpType = type;
        }
        protected virtual void SetTexts()
        {
            titleHolder.text = Data.Title;
            priceHolder.text = Data.Price.ToString();
        }

       
    }

}
