using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace DataModels
{
    [CreateAssetMenu(fileName = nameof(TurnButtonHolder))]
    public class TurnButtonHolder : ScriptableObject
    {
        [SerializeField] private ButtonData[] buttonContents;
        public Dictionary<TurnStateType, ButtonData> ButtonsByType = new ();
        
        public void Setup()
        {
            foreach (var button in buttonContents)
            {
                ButtonsByType.Add(button.StateType, button);
            }
        }

    }
    
    [Serializable]
    public class ButtonData
    {
        public TurnStateType StateType;
        public string Content;
    }
    
}
