using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public static class GeneralEventbus
{


    public static Action OnTowersCreated;
    public static Action OnTowersAndTeamsReady;
    public static Action<int, int> OnHealthIconChangeRequest;
    public static Action<IEnumeratorContainer> OnCoroutineTrigger;
    
    public static Action<int> OnTurnTowerSelection;
    public static Action<int> OnTurnTowerDeselect;

}
