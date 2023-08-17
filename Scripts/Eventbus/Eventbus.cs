using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class Eventbus
{

    public static class TowerEvents
    {
        public static Action<Tower> OnTowerClicked;
    }

    public static class UIEvents
    {
        public static Action<bool> OnButtonCall;
    }

    public static class FireEvents
    {
        public static Action OnFireEnabled;
        public static Action<List<CombatPair>> OnPairsAltered;
    }

    public static class TowerGroupEvents
    {
        public static Action OnTowersAltered;
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
