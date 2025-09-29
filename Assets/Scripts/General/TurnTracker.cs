using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurnTracker
{
    private static int Tracker { get; set; } = 0;
    public static int GetTurnTracker() => Tracker;

    public static void IncreaseTracker()
    {
        Tracker++;
        //Debug.Log("turn track: " + Tracker);
    }

   
}
