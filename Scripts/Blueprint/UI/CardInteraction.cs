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
        public GameObject blocker;
        
        public void Setup(BlueprintData currentData) //belki de burda olmamalı
        {
            _currentBpData = currentData;
        }
        
        private void OnMouseDown()
        {
            if(!isInteractable) return;
            print("select: "+_currentBpData.Type);
            BpEventbus.SelectionEvents.OnCardClicked?.Invoke(_currentBpData.Type);
        }
        
        void Select()
        {
            //TODO: öne geçme animasyonu
        }
        public void Activate()
        {
            isInteractable = true;
            blocker.SetActive(false);
        }

        public void Deactivate()
        {
            isInteractable = false;
            blocker.gameObject.SetActive(true);
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
