using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommunEventbus 
{
    public static class ChainTurnEvents
    {
        public static Action<int[]> OnLinkedTowers;
        public static Action OnLinkBroken;
        public static Action<float> OnRising;
    }
}
