using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace GameUI
{
    public class UIEventbus
    {
        public static Action<bool> OnButtonCall;
        public static Action<bool, TurnStateType> OnShowButtonRequest;
        public static Action OnButtonClicked;
        
        public static Action<float, GameObject> OnTowerHeightChange;
        public static Action<int, GameObject> OnHealthChange;
        public static Action<TeamType> OnTeamSwitch;

        public static Action<string> OnPlayerSet;
        
        public static Action<GameObject> OnTeamChange;
        
        
    }
    
    
}


