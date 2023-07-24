using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Eventbus
{
    public class TurnEvents
    {
        public static Action OnTurnStarted;
        public static Action OnTurnEnded;
        public static Action<List<Tower>> OnSelectionEnded;

    }

    public class TowerEvents
    {
        public static Action<Tower> OnTowerClicked;
    }
    
}
