using System.Collections;
using System.Collections.Generic;
using Network;
using Turn;
using UnityEngine;

public class BlueprintEventHandler
{
    private TurnManager _manager;
    public BlueprintEventHandler(TurnManager manager)
    {
        _manager = manager;
        SubscribeToBlueprintEvents();
    }
    
    void SubscribeToBlueprintEvents()
    {
        BpEventbus.ActionEvents.OnReverseActionTriggered += PublishReverseOrderAction;
    }

    void PublishReverseOrderAction()
    {
        BpEventbus.SubscriberEvents.OnReverseAction?.Invoke();
    }
    
    public void UnsubscribeFromBlueprintEvents()
    {
        BpEventbus.ActionEvents.OnReverseActionTriggered -= PublishReverseOrderAction;
    }
}
