using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public static class TriggerEvents
    {
        public static Action<List<int>> OnBpCompletedByButton;
    }
    
 
}
