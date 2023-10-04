using System;
using System.Collections.Generic;
using Data;
using DataModels;
using Teams;
using Towers;

public static class Eventbus
{
   
    public static class TeamEvents
    {
        public static Action<Team[]> OnTeamsSet;
        public static Action<TowerData> OnTeamChange;
    }

    public static class CombatEvents
    {
        public static Action<TowerData> OnTowerKilled;
        public static Action OnMatchesRestored;
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