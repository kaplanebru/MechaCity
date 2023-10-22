using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [CreateAssetMenu(fileName = nameof(EditorEventHandler))]
    public class EditorEventHandler : ScriptableObject
    {
        public Action OnTest;
        public void RaiseEvent()
        {
            OnTest?.Invoke();
            Debug.Log("event raised");
        }
    }
    
   

}
 
