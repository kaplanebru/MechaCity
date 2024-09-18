using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BridgeEventListener : TowerRelatedEventListener<BridgeRoot>
{
    protected override BridgeRoot[] RelatedItems { get; set; }

    public override void Initialize()
    {
    }

    public override void Subscribe()
    {
        Eventbus.TowerEvents.OnBridgeAttempt += ConstructBridge;
    }


    void ConstructBridge(int[] ids)
    {
        var bridge = RelatedItems.FirstOrDefault(s => s.Id == ids[0]);
        bridge.Stretch(ids[1]);
    }

    public override void Unsubscribe()
    {
        Eventbus.TowerEvents.OnBridgeAttempt -= ConstructBridge;
    }
}