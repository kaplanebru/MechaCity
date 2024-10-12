using System;
using System.Collections.Generic;
using Enums;

public static class Eventbus
{
    public static class ActorEvents
    {
        public static Action<int[]> OnNewDoubleActor;
    }
    public static class CombatEvents
    {
        public static Action<List<int>> OnTowerKilled;
        public static Action <float> OnNextTower;
        public static Action OnPairsSet;
        
        public static Action OnCombatStarted;
        public static Action OnCombatReady;
        public static Action OnCombatEnding;
        public static Action OnCombatTerminated;

        public static Action<int> OnTurnTowerSelection;
        public static Action OnTurnTowerDeselect;
    }

    public static class LinkEvents
    {
        public static Action OnLinkStateBegin;
        public static Action<List<int>> OnLinkLoading;
        public static Action<List<int>> OnLinkingTowers;
        public static Action<List<int>> OnUnlink;
        public static Action OnFloorsOpened;

        public static Action <List<string>> OnCreatingCombatPairs;
    }
    public static class HealthEvents
    {
        public static Action<string, int, Action> OnShoot;
        public static Action<string> OnHealthChange;
        public static Action OnHealthsSet;
        public static Action<string> OnRemoveFromRegistry;
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