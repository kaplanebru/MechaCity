using System;
using System.Collections.Generic;
using Chain;
using UnityEngine;

public class ChainEvents
{
    public static Action<List<Vector3>> OnPointsCreated;
    public static Action<List<ChainLink>, List<Vector3>> OnLinksCreated;
    public static Action<bool> OnMotionStateSet; //todo: centralize from machinery
    
    public static Action<int, float> OnCogSpeedSet; //todo: centralize from machinery

    public static Action OnCogSetupRequest; //todo: centralize from machinery
    public static Action<CogData, Transform> OnCogDataSet;
    public static Action<Cogwheel[], ChainSpawner> OnChainRequest;

    
    
    public static Action<CogData> OnNewCogData;

    public static Action<CogData, Transform> OnPoolReady;

    public static Action OnDeleteLinks;
    public static Action OnDeleteTeeth;
    
    

    public static Action<Transform> OnDeleteObject;

    public static Action OnLinksReady;

}