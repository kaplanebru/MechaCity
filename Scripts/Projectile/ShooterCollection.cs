using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using UnityEngine;

public class ShooterCollection : TowerRelatedElementCollection<Shooter>
{
    public CombatTimingData timingData;

    public override void Subscribe()
    {
        CombatPairEvents.OnShoot += ShootByGivenShooter;
    }

    public override void Initialize()
    {
        foreach (var shooter in Collection.Values)
        {
            shooter.SetDuration(timingData.shooterMotionDuration, timingData.ProjectileDuration);
        }
    }

    private void ShootByGivenShooter(CombatPair pair)
    {
        var shooter = Collection[pair.MainTowerData.UniqID];
        shooter.Shoot(pair);
    }
    
    public override void Unsubscribe()
    {
        CombatPairEvents.OnShoot -= ShootByGivenShooter;
    }
}
