using System;
using System.Collections;
using System.Collections.Generic;
using Blueprint;
using Enums;
using Enums.Selections;
using Towers;
using UnityEngine;

public static class BpEventbus
{

    public static Action<uint[]> OnSendingSelectionsForExecution;
    public static Action<uint[]> OnDirectBpExecution;
    public class SelectionEvents
    {
        public static Action<SelectionType> OnCurrentBpSet;
    }

    public class StateEvents
    {
        public static Action<bool> OnDirectStateChangeFromIntruder;
        public static Action<TurnStateType> StateChangeRequestToIntruder;

        public static Action OnIntruderExecutionAttempt;
    }
    public class ActionEvents
    {
        public static Action OnReverseActionTriggered;
        public static Action OnSelectionIncrementTriggered;
        public static Action OnRestoreSelectionAmount;
    }

    public static class SubscriberEvents
    {
        public static Action OnSelectionIncrease;
        public static Action OnSelectionRestoration;
    }
    
    public static class LifespanEvents
    {
        public static Action<BpType, uint> OnRestore;
        public static Action<ITrackable> OnTrackerRequest;
        public static Action<ITrackable> OnExpiredTracker;
    }
    public static class UIEvents
    {
        public static Action<BpType, int> OnInteraction;
        public static Action<BpType> OnBpInstalled;
        public static Action<BpType> OnBpInstallBegin;
        public static Action OnBpReset;
    }
    
 
}
