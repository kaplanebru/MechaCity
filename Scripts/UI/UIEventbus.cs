using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace GameUI
{
    public class UIEventbus
    {
        public static Action<bool, TurnStateType> OnButtonCall;
        public static Action<float, GameObject> OnTowerHeightChange;
        public static Action<int, GameObject> OnHealthChange;
        public static Action<TeamType> OnTeamSwitch;
        
        public static class TurnEvents
        {
            public static Action OnInitialize;
            public static Action<bool> OnTurnButtonShiftRequest;
        }
    }
    
    
}


