using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BridgeEventListener : TowerRelatedEventListener<BridgeRoot>
{
    protected override BridgeRoot[] RelatedItems { get; set; }

    public override void Initialize()
    {
       DisableAll();
    }

    public override void Subscribe()
    {
        Eventbus.TowerEvents.OnBridgeAttempt += ConstructBridge;
    }


    void ConstructBridge(int[] ids)
    {
        Debug.Log(RelatedItems.Length);
        for (int i = 0; i < ids.Length-1; i++)
        {
            var bridge = RelatedItems.FirstOrDefault(s => s.Id == ids[i]);
            bridge.Show(true);
            bridge.Stretch(ids[i+1]);
        }
    }

    void DisableAll()
    {
        foreach (var relatedItem in RelatedItems)
        {
            relatedItem.Show(false);
        }
    }

    public override void Unsubscribe()
    {
        Eventbus.TowerEvents.OnBridgeAttempt -= ConstructBridge;
    }
}