using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class Slot
{
    public bool HasTower;
    public int Number;
    public Tower Tower;

    // public void OnSlotEnabled()
    // {
    //     Eventbus.FireEvents.OnTowerKilled += RemoveTower;
    // }
    
    // void RemoveTower(Tower tower)
    // {
    //     if (tower != Tower) return;
    //     
    //     HasTower = false;
    // }
    // public void OnSlotDisabled()
    // {
    //     Eventbus.FireEvents.OnTowerKilled -= RemoveTower;
    // }

    
}
