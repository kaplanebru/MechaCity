using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerOrienter : MonoBehaviour
{
    public Transform center;

    private void OnEnable()
    {
        GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady += Orient;
    }

    void Orient()
    {
        GeneralEventbus.InitializerEvents.OnOrienterReady?.Invoke(center.position);
    }

    private void OnDisable()
    {
        GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady -= Orient;
    }
}