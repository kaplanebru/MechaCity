using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class Slot
{
    public bool hasTower;
    public int Number;
    public Tower Tower;

    public void OnSlotEnabled()
    {
        Eventbus.FireEvents.OnTowerDied += RemoveTower;
    }
    
    void RemoveTower(Tower tower)
    {
        if (tower == Tower)
            hasTower = false;
    }
    public void OnSlotDisabled()
    {
        Eventbus.FireEvents.OnTowerDied -= RemoveTower;
    }

    
}
