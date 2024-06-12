using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocksEventListener : MonoBehaviour
{
    private Lock[] lokcs; //bir towerda 1'den fazla lock olabilir. Lockholderları get edelim
    private void OnEnable()
    {
        GeneralEventbus.OnTowersCreated += GetLocks;
    }

    private void GetLocks()
    {
        
    }

    private void OnDisable()
    {
        GeneralEventbus.OnTowersCreated -= GetLocks;
    }
}
