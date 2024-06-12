using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using UnityEngine;

public class ShootersEventListener : TowerRelatedEventListener<Shooter>
{
    public CombatTimingData timingData;
    protected override Shooter[] RelatedItems { get; set; }
    public override void Subscribe()
    {
        CombatPairEvents.OnShoot += ShootByGivenShooter;
    }

    public override void Initialize()
    {
        print(RelatedItems.Length);
        foreach (var shooter in RelatedItems)
        {
            shooter.SetDuration(timingData.shooterMotionDuration, timingData.ProjectileDuration);
        }
    }

    private void ShootByGivenShooter(CombatPair pair)
    {
        var shooter = RelatedItems.FirstOrDefault(s => s.Id == pair.MainTowerData.UniqID);
        shooter.Shoot(pair);
    }
    
    public override void Unsubscribe()
    {
        CombatPairEvents.OnShoot -= ShootByGivenShooter;
    }
}
