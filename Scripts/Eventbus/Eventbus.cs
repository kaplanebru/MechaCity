using System;
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
        public static Action <float> OnFire;
        public static Action OnCombatStarted;
        public static Action OnCombatReady;
        public static Action OnCombatEnding;
        public static Action OnCombatTerminated;
        public static Action OnPairsSet;

        public static Action<IEnumeratorContainer> OnCoroutineTrigger;
    }

    public static class StateEvents
    {
        public static Action OnLinkStateBegin;
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