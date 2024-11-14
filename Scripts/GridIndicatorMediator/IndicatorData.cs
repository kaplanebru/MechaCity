using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class IndicatorData //ortak asmdf yapılır
{
    public Vector3 StartPos;
    public List<Vector3> TargetPositions = new();
    public Dictionary<Vector3, IndicatorState> TargetPosStates = new();
}

public static class IndicatorEvents
{
    public static Action<List<IndicatorData>> OnIndicatorDatasSet;
}