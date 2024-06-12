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
        GeneralEventbus.OnShoot += RevealShooter;
    }
    
    private void GetShooters()
    {
        shooters = GetComponentsInChildren<Shooter>();
        foreach (var shooter in shooters)
        {
            shooter.SetDuration(timingData.shooterDuration);
        }
    }

    private void RevealShooter(int id)
    {
        var shooter = shooters.FirstOrDefault(s => s.Id == id);
        shooter.RevealSelf();
    }
    
    private void OnDisable()
    {
        GeneralEventbus.OnTowersCreated -= GetShooters;
        GeneralEventbus.OnShoot -= RevealShooter;

    }
}
