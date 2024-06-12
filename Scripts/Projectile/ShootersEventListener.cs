using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using UnityEngine;

public class ShootersEventListener : MonoBehaviour
{
    public Shooter[] shooters;
    public CombatTimingData timingData;
    private void OnEnable()
    {
        GeneralEventbus.OnTowersCreated += GetShooters;
        CombatPairEvents.OnShoot += ShootByGivenShooter;
    }
    
    private void GetShooters()
    {
        shooters = GetComponentsInChildren<Shooter>();
        foreach (var shooter in shooters)
        {
            shooter.SetDuration(timingData.shooterMotionDuration, timingData.ProjectileDuration);
        }
    }

    private void ShootByGivenShooter(CombatPair pair)
    {
        var shooter = shooters.FirstOrDefault(s => s.Id == pair.MainTowerData.UniqID);
        shooter.Shoot(pair);
    }
    
    private void OnDisable()
    {
        GeneralEventbus.OnTowersCreated -= GetShooters;
        CombatPairEvents.OnShoot -= ShootByGivenShooter;
    }
}
