using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BridgeCollection : TowerRelatedElementCollection<BridgeRoot>
{
    //protected override BridgeRoot[] RelatedItems { get; set; }

    public override void Initialize()
    {
       DisableAll();
    }

    protected override Dictionary<int, BridgeRoot> Collection { get; set; } = new();

    public override void Subscribe()
    {
        Eventbus.TowerEvents.OnBridgeAttempt += ConstructBridge;
    }


    void ConstructBridge(int[] ids)
    {
        for (int i = 0; i < ids.Length-1; i++)
        {
            // var bridge = RelatedItems.FirstOrDefault(s => s.Id == ids[i]); //todo: dict yap
            // var target = RelatedItems.FirstOrDefault(s => s.Id == ids[i + 1]);
            var bridge = Collection[ids[i]];
            var target = Collection[ids[i + 1]];
            bridge.Show(true);
            bridge.Stretch(target.Id); //ids[i+1]
        }
    }

    void DisableAll()
    {
        foreach (var relatedItem in Collection.Values)
        {
            relatedItem.Show(false);
        }
    }

    public override void Unsubscribe()
    {
        Eventbus.TowerEvents.OnBridgeAttempt -= ConstructBridge;
    }
}