using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


namespace ClickHandler
{
    public abstract class BaseClickable <T> : MonoBehaviour
    {
        public T clickableObject; // { get; set; }
   

        private void OnMouseDown() //disabled for MP
        {
            //Eventbus.InputEvents.OnObjectClicked?.Invoke(new object[] {clickableObject});
        }

        public abstract void UnsubscribeFromEvent();

        private void OnEnable()
        {
            Setup();
        }
   
        protected virtual void Setup() {}

        protected virtual void Setup([CanBeNull]T obj)
        {
            clickableObject = obj;
        }

        private void OnDisable()
        {
            UnsubscribeFromEvent();
        }
    }

}
