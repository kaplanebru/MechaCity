using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using Enums;
using TMPro;
using UnityEngine;

namespace Blueprint
{
    public class CardSlotFront : CardSlot
    {
        public GameObject reliefModel;
        public TextMeshPro descriptionHolder;
        public GameObject applyButton;
        
        
        public override void Setup(BlueprintData data)
        {
            base.Setup(data);
            SetReliefModel();
        }
        public void SetReliefModel()
        {
            reliefModel = Data.ReliefModel;
            //instantiate?
        }
        
        protected override void SetTexts()
        {
            base.SetTexts();
            descriptionHolder.text = Data.Description;
        }

        public void ApplyCard()
        {
            cardInteraction.ApplyCard();
        }

        public override void Activate()
        {
            base.Activate();
            applyButton.SetActive(true);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            applyButton.SetActive(false);
        }

    }

}
