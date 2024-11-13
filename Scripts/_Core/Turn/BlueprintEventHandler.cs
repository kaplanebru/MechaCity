using Turn;
using UnityEngine;

public class BlueprintEventHandler
{
    
    public BlueprintEventHandler()
    {
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
        Eventbus.ActorEvents.OnReverseGrid?.Invoke();
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
