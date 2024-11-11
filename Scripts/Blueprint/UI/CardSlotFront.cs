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
        
        public override void Setup(BlueprintData data)
        {
            base.Setup(data);
            SetReliefModel();
        }
        
        void SetReliefModel()
        {
            reliefModel = Data.ReliefModel;
            //instantiate?
        }
        
        protected override void SetTexts()
        {
            base.SetTexts();
            descriptionHolder.text = Data.Description;
        }
    }

}
