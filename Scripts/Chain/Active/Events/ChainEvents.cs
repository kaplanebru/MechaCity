using System;
using System.Collections.Generic;
using Chain;
using UnityEngine;

public class ChainEvents
{
    public static Action<List<Vector3>> OnPointsCreated;
    public static Action<List<Transform>> OnLinksCreated;
    public static Action<bool> OnMotionStateSet;
    
    public static Action<int, float> OnCogSpeedSet;

    public static Action OnCogSetupRequest;
    public static Action<CogData, Transform> OnCogDataSet;
    public static Action<Cogwheel> OnCogReady;

    public static Action OnPoolCreated;
}