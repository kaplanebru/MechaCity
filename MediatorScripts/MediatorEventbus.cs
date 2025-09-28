using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MediatorEventbus 
{
    public static class ChainLinkEvents
    {
        public static Action<int[]> OnLinkedTowers;
        public static Action<int[]> OnFloorsOpened;
        public static Action OnLinkBroken;
    }
    
    public static class ChainMotionEvents
    {
        public static Action OnMotion;
        public static Action OnStop;
    }

    public static class EffectEvents
    {
        public static Action<int> OnDeathEffect;
    }

    public static class SetupEvents
    {
        public static Action<IGear[]> OnGearsReady;
        public static Action<int, GameObject> OnTowerIDSetting;
    }
}


public interface IGear
{
    public GameObject GameObject { get; set; }
}