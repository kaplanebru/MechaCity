using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public static class GeneralEventbus
{

    public static class InitializerEvents
    {
        public static Action OnInitialize;
        public static Action OnTowersCreated;
        public static Action OnTowerRelatedIDsSet;
        public static Action OnTowersAndTeamsReady;
        public static Action OnExternalElementsReady;
    }

    public static Action<IEnumeratorContainer> OnCoroutineTrigger;
    
    public static Action<int> OnTowerColorChange;
    public static Action<int> OnTurnTowerDeselect;
    
    public static Action OnResetMaxSelectionFromEditor;
    
    public static class IndicatorEvents
    {
        public static Action<uint> OnActorHover;
        public static Action OnLeavingActor;
        public static Action<Dictionary<uint, List<Vector3>>> OnActorsResolved;
        
        
    }

}
