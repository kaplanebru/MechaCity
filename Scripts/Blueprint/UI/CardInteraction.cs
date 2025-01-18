using System.Collections;
using System.Collections.Generic;
using DataModels;
using UnityEngine;

namespace Blueprint
{
    public class CardInteraction : MonoBehaviour
    {
        private BlueprintData _currentBpData;
        public bool isInteractable = true;
        
        public void Setup(BlueprintData currentData) //belki de burda olmamalı
        {
            _currentBpData = currentData;
        }
        
        private void OnMouseDown()
        {
            if(!isInteractable) return;
            Debug.Log("SELECT");
            Select();
        }
        
        void Select()
        {
            BpEventbus.CardEvents.OnCardSelection?.Invoke(_currentBpData.Type);
            //TODO: öne geçme animasyonu
        }
        internal void ApplyCard()
        {
            if(!isInteractable) return;
            print("select: "+_currentBpData.Type);
            BpEventbus.SelectionEvents.OnCardSelectionApplied?.Invoke(_currentBpData.Type);
        }

        // private void OnMouseEnter()
        // {
        //     //HoverImage();
        //    // print("select: "+_currentBpData.Type);
        // }
        //
        //
        // private void OnMouseExit()
        // {
        //     //ResetImage();
        // }
        
      
    }

}
