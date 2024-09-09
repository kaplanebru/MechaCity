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


    void ConstructBridge(int id1, int id2)
    {
        var bridge = RelatedItems.FirstOrDefault(s => s.Id == id1);
        bridge.Stretch(id2);
    }

    public override void Unsubscribe()
    {
        Eventbus.TowerEvents.OnBridgeAttempt -= ConstructBridge;
    }
}