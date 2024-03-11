using System;
using System.Collections;
using System.Collections.Generic;
using Blueprint;
using Enums;
using UnityEngine;

public static class BpEventbus 
{
    public class ActionEvents
    {
        public static Action OnReverseActionTriggered;
    }

    public static class SubscriberEvents
    {
        public static Action OnReverseAction;
    }
    
    public static class LifespanEvents
    {
        public static Action<BpLifeTracker> OnBpExpired;
        public static Action<BpType> OnBpAdded;
        public static Action<BpType> OnRestore;
    }
    
    public static class UIEvents
    {
        public static Action<BpType> OnBpInstalled;
        public static Action<BpType> OnBpInstallBegin;
    }
    
 
}
