using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BridgeCollection : TowerRelatedElementCollection<BridgeRoot>
{
    private List<BridgeGroup> bridgeGroups = new();
    public override void Initialize()
    {
       DisableAll();
    }
    
    public override void Subscribe()
    {
        Eventbus.TowerEvents.OnBridgeAttempt += ConstructBridge;
        Eventbus.TowerEvents.OnBridgeDestroyRequest += RemoveBridge;
    }


    void ConstructBridge(int[] towerIDs)
    {
        CreateBridgeGroups(towerIDs);
        foreach (var bridgeGroup in bridgeGroups)
        {
            var bridge = Collection[bridgeGroup.First];
            var target = Collection[bridgeGroup.Second];
            
            bridge.Show(true);
            bridge.Stretch(target.Id);
        }
    }

    void CreateBridgeGroups(int[] towerIDs)
    {
        bridgeGroups.Clear();
        towerIDs = towerIDs.OrderBy(id => id).ToArray();
        
        for (int i = 0; i < towerIDs.Length; i++)
        {
            if(i+1 == towerIDs.Length) break;
            bridgeGroups.Add(new BridgeGroup(towerIDs[i], towerIDs[i+1]));
        }
    }
  

    void RemoveBridge(int id)
    {
        var bridge = Collection[id];
        bridge.RemoveBridge();
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
        Eventbus.TowerEvents.OnBridgeDestroyRequest -= RemoveBridge;
    }
}