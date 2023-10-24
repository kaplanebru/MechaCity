using System;
using System.Collections.Generic;
using Chain;
using UnityEngine;

public class ChainEvents
{
    public static Action<List<Vector3>> OnPointsCreated;
    public static Action<List<Transform>> OnLinksCreated;
    public static Action<bool> OnMotionStateSet;
    
    public static Action<int, Transform, float> OnTeethCreated;
    public static Action<int, float> OnCogSpeedSet;

    public static Action OnCogSetupRequest;
    //public static Action<CogData, Transform> OnCogReady;
    public static Action<object[]> OnCogReady;
}