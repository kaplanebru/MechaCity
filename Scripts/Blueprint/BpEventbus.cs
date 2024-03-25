using System;
using System.Collections;
using System.Collections.Generic;
using Blueprint;
using Enums;
using UnityEngine;

public static class BpEventbus 
{
    public class SettingEvents
    {
        public static Action<SelectionType, int> OnCurrentBpSet;
    }
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
  
        public static Action<BpType, int> OnRestore;
        
        public static Action<ITrackable> OnTrackerRequest;
        public static Action<ITrackable> OnExpiredTracker;

    }
    
    public static class UIEvents
    {
        public static Action<BpType, int> OnInteraction;
        public static Action<BpType> OnBpInstalled;
        public static Action<BpType> OnBpInstallBegin;
    }
    
 
}
