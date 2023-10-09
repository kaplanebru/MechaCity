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
        public static Action<float, GameObject> OnTowerHeightChange;
        public static Action<int, GameObject> OnHealthChange;
        public static Action<TeamType> OnTeamSwitch;
        
        public static class TurnEvents
        {
            public static Action OnInitialize;
        }
    }
    
    
}


