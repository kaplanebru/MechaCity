using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class IndicatorGridData //ortak asmdf yapılır
{
    public uint ActorID;
    public Vector3 StartPos;
    public List<Vector3> TargetPositions = new();
    public Dictionary<Vector3, IndicatorState> TargetStates = new();
}

public static class IndicatorEvents
{
    public static Action<List<IndicatorGridData>> OnIndicatorGridDatasSet;
}