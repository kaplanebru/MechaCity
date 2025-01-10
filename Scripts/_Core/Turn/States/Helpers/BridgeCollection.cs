using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Towers;
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
    // void ConstructBridge(int[] ids)
    // {
    //     for (int i = 0; i < ids.Length-1; i++)
    //     {
    //         var bridge = Collection[ids[i]];
    //         var target = Collection[ids[i + 1]];
    //         bridge.Show(true);
    //         bridge.Stretch(target.Id); //ids[i+1]
    //     }
    // }

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

public class BridgeGroup
{
    public int First;
    public int Second;

    private TowerNumericData firstTower;
    private TowerNumericData secondTower;

    public BridgeGroup(int first, int second)
    {
        First = first;
        Second = second;
        GetTowers();
        ReorderByHeight();
    }

    void GetTowers()
    {
        firstTower = AllTowers.GetNumericData(First);
        secondTower = AllTowers.GetNumericData(Second);
    }

    void ReorderByHeight()
    {
        if (firstTower.Height > secondTower.Height)
            (First, Second) = (Second, First);
    }
}