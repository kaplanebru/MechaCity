using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameUI;
using UnityEngine;

public class HealthHoldersEventListener : TowerRelatedEventListener<HealthHolder>
{
    protected override HealthHolder[] RelatedItems { get; set; }
    public HealthHolder healthHolder;

    public override void Subscribe()
    {
        GeneralEventbus.OnHealthIconChangeRequest += AdjustHealthIcon;
        GeneralEventbus.OnCommonHealthIconRequest += CreateCommonIcon;
    }

    public override void Initialize() { }
    
    private void AdjustHealthIcon(int health, int id)
    {
        var healthHolder = RelatedItems.FirstOrDefault(h => h.Id == id);
        
        healthHolder.AdjustIcons(health);
    }

    public void CreateCommonIcon(int[] ids, int totalHealth)
    {
        //ids = ids.OrderBy(id=>id).ToArray();
        HealthHolder[] holders = new HealthHolder[ids.Length];
        Vector3 center = Vector3.zero;
        
        for (var i = 0; i < ids.Length; i++)
        {
            holders[i] = RelatedItems.FirstOrDefault(h => h.Id == ids[i]);
            holders[i].DisableAll();
            center += holders[i].transform.position;
        }

        center /= holders.Length;

        var health = Instantiate(healthHolder, center, Quaternion.identity); //TODO: health holder prefab. Double' ile birlikte inip çıkmalı, o yüzden en yüksek olanın tepesine koy!
        health.AdjustIcons(totalHealth);
    }

    public override void Unsubscribe()
    {
        GeneralEventbus.OnHealthIconChangeRequest -= AdjustHealthIcon;
        GeneralEventbus.OnCommonHealthIconRequest -= CreateCommonIcon;
    }
}