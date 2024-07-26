using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MediatorEventbus 
{
    public static class ChainTurnEvents
    {
        public static Action OnTowersAndTeamsReady;
        public static Action<int[]> OnLinkedTowers;
        public static Action OnLinkBroken;
        public static Action OnRising;
        public static Action OnStop;
    }

    public static class EffectEvents
    {
        public static Action<int> OnDeathEffect;
    }

    public static class SetupEvents
    {
        public static Action<IGear[]> OnGearsReady;
    }
}


public interface IGear
{
    public GameObject GameObject { get; set; }
}