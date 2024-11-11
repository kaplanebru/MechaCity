using System.Collections;
using System.Collections.Generic;
using DataModels;
using UnityEngine;

namespace Blueprint
{
    public class CardInteraction : MonoBehaviour
    {
        private BlueprintData _currentBpData;
        
        public void Setup(BlueprintData currentData) //belki de burda olmamalı
        {
            _currentBpData = currentData;
        }
        
        private void OnMouseDown()
        {
            print("select: "+_currentBpData.Type);
            Select();
        }


        private void OnMouseEnter()
        {
            //HoverImage();
           // print("select: "+_currentBpData.Type);
        }
    
       
        private void OnMouseExit()
        {
            //ResetImage();
        }
        
        void Select()
        {
            //TODO: öne geçme animasyonu
            BpEventbus.UIEvents.OnInteraction?.Invoke(_currentBpData.Type, _currentBpData.Level); //sadece manager dinliyor, slota event atamaz bütün slotlara gider
        }
    }

}
