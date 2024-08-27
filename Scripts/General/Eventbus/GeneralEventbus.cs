using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public static class GeneralEventbus
{

    public static class InitializerEvents
    {
        public static Action OnTowersCreated;
        public static Action OnTowersAndTeamsReady;
        public static Action OnExternalElementsReady;
    }

    public static Action<int, int> OnHealthIconChangeRequest;
    public static Action<IEnumeratorContainer> OnCoroutineTrigger;
    
    public static Action<int> OnTowerColorChange;
    public static Action<int> OnTurnTowerDeselect;

}
