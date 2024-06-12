using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthHoldersEventListener : TowerRelatedEventListener<HealthHolder>
{
    protected override HealthHolder[] RelatedItems { get; set; }
    public override void Subscribe()
    {
        GeneralEventbus.OnHealthIconChangeRequest += AdjustHealthIcon;
    }

    public override void Initialize() { }
    
    private void AdjustHealthIcon(int health, int id)
    {
        var healthHolder = RelatedItems.FirstOrDefault(h => h.Id == id) as HealthHolder;
        healthHolder.AdjustIcons(health);
    }

    public override void Unsubscribe()
    {
        GeneralEventbus.OnHealthIconChangeRequest -= AdjustHealthIcon;
    }
}