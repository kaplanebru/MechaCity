using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocksEventListener : TowerRelatedEventListener<LockHolder>
{
    protected override LockHolder[] RelatedItems { get; set; }
    public override void Subscribe()
    {
        Eventbus.TowerEvents.OnLock += LockGivenTower;
    }

    private void LockGivenTower(int limit, int id)
    {
        var lockHolder = RelatedItems.FirstOrDefault(l => l.Id == id);
        lockHolder.LockTower(limit);
    }

    public override void Initialize()
    {
        
    }

    public override void Unsubscribe()
    {
        Eventbus.TowerEvents.OnLock -= LockGivenTower;

    }
}
