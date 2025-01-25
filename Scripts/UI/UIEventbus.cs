using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace GameUI
{
    public class UIEventbus
    {
        public static Action<bool> OnApplyPossibility;
        public static Action<TurnStateType> OnStateShift;
        public static Action<bool> OnHighlightRequest;
        public static Action OnButtonClicked;
        
        public static Action<int> OnTowerHeightChange;
        
       // public static Action<TeamType> OnActiveTeamSet;

        
        public static Action<GameObject> OnTeamChange;
        public static Action<PopupType> OnPopupTime;


    }
    
    
}


