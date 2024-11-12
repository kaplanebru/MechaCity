using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public static class Eventbus
{
    public static class ActorEvents
    {
        public static Action<uint[]> OnDoubleTowerCreated;
        public static Action<uint[]> OnRegistryUpdate;
        public static Action<List<uint>, bool> OnRelationsSet;
        public static Action OnReverseRelations;
    }
    public static class CombatEvents
    {
        public static Action<uint> OnActorKilled;
        public static Action<uint> OnTeamSwitch;
        public static Action <float> OnNextActor;
        public static Action <bool> OnPairsSet;
        
        public static Action OnCombatStarted;
        public static Action OnCombatReady;
        public static Action OnCombatEnding;
        public static Action OnCombatTerminated;

        public static Action<uint> OnTurnTowerSelection;
        public static Action OnTurnTowerDeselect;
    }

    public static class LinkEvents
    {
        public static Action OnLinkStateBegin;
        public static Action<List<int>> OnLinkLoading;
        public static Action<List<uint>> OnLinkActorsLoaded;
        public static Action<List<int>> OnLinkingTowers;
        public static Action<List<int>> OnUnlink;
        public static Action OnFloorsOpened;
    }
    
    public static class SelectionEvents
    {
        public static Action OnSelectionStateBegin;
    }
    public static class HealthEvents
    {
        public static Action<uint, int, Action> OnShoot;
        public static Action<uint> OnHealthChange;
        public static Action<int[]> OnRemoveFromRegistry;
    }
    public static class TowerEvents
    {
        public static Action<int, int> OnLock;
        public static Action OnTurnBegin;
        public static Action<int[]> OnBridgeAttempt; 
    }
    
   






    // public class SubscriptionModel
    // {
    //     public string SubscriberName { get; set; }
    //     public bool IsActive { get; set; }
    //     public Subscription Subscription { get; set; }
    // }
    //
    // public delegate void Subscription(params object[] args); //where TManager : Manager<TManager>;

    // public void Publish(string eventName, params object[] args)
    // {
    //     if (!Subscriptions.TryGetValue(eventName, out var eventSubscriptions)) return;
    //
    //     eventSubscriptions = eventSubscriptions.Where(x => x.IsActive).ToList();
    //
    //     foreach (var subscription in eventSubscriptions)
    //         subscription.Subscription.Invoke(args);
    // }
}