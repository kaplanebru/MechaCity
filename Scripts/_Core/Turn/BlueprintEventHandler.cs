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
        BpEventbus.ActionEvents.OnSelectionIncrementTriggered += PublishSelectionIncrementAction;
        BpEventbus.ActionEvents.OnRestoreSelectionAmount += PublishSelectionRestoration;

    }

    void PublishReverseOrderAction()
    {
        Debug.Log("publish reverse");
        BpEventbus.SubscriberEvents.OnReverseAction?.Invoke();
    }

    void PublishSelectionIncrementAction()
    {
        
        BpEventbus.SubscriberEvents.OnSelectionIncrease?.Invoke();
    }

    void PublishSelectionRestoration()
    {
        //Debug.Log("publish restore");
        BpEventbus.SubscriberEvents.OnSelectionRestoration?.Invoke();
    }
    
    public void UnsubscribeFromBlueprintEvents()
    {
        BpEventbus.ActionEvents.OnReverseActionTriggered -= PublishReverseOrderAction;
        BpEventbus.ActionEvents.OnSelectionIncrementTriggered -= PublishSelectionIncrementAction;
        BpEventbus.ActionEvents.OnRestoreSelectionAmount -= PublishSelectionRestoration;

    }
}
