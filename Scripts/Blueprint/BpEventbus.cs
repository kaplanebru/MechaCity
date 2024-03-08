using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public static class ManagementEvents
    {
        public static Action<BpType> OnBpExpired;
    }
    
    public static class UIEvents
    {
        public static Action<BpType> OnBpInstalled;
        public static Action<BpType> OnBpInstallBegin;
    }
    
 
}
