using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public static class GeneralEventbus
{

    public static class InitializerEvents
    {
        public static Action OnActorsCreated;
        public static Action OnTowerRelatedIDsSet;
        public static Action OnMediatorElementsReady;

        public static Action<Vector3> OnOrienterReady;
    }

    public static Action<IEnumeratorContainer> OnCoroutineTrigger;
    
    public static Action<int> OnTowerColorChange;
    public static Action<int> OnTurnTowerDeselect;
    
    public static Action OnResetMaxSelectionFromEditor;
    
    public static class IndicatorEvents
    {
        public static Action<uint> OnActorHoverByUser;
        public static Action OnActorLeftByUser;

        public static Action<uint> OnActorHoverByCombat;
        public static Action OnActorLeftByCombat;
        public static Action<Dictionary<uint, List<Vector3>>> OnActorsEdgesRestored;
    }

}
