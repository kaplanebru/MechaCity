using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainEvents
{
    public static Action<List<Vector3>> OnPointsCreated;
    public static Action<List<Transform>> OnLinksCreated;
    public static Action<bool> OnStartAndMove;
}