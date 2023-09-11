using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using Models;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class Eventbus
{

    public static class TowerEvents
    {
        public static Action<Tower> OnTowerSetup;
    }

    public static class TeamEvents
    {
        public static Action<Tower> OnTeamChange;
    }

    public static class UIEvents
    {
        public static Action<bool> OnButtonCall;
        public static Action<float, GameObject> OnTowerHeightChange;
        public static Action<int, Tower> OnHealthChange;
    }

    public static class InputEvents //TODO: bu map node'da nasıl handle ediliyordu
    {
        public static Action<object[]> OnObjectClicked;
    }

    public static class NetworkEvents
    {
        public static Action<Player, ulong> OnPlayerSpawned;
    }
    

    public static class TurnEvents
    {
        public static Action OnTurnCompleted;
    }

    public static class FireEvents
    {
        public static Action OnFireEnabled;
        public static Action<Vector3> OnShooting;
        public static Action<Tower> OnTowerKilled;
        public static Action<TowerGridRelationModel> OnTowerTeamDetection;
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
