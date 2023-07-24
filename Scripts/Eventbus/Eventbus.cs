using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Eventbus
{
    public static class TurnEvents
    {
        public static Action<object[]> OnTurnStateChanged;
        public static Action OnTurnEnded;
        public static Action<List<Tower>> OnSelectionEnded;

    }

    public static class TowerEvents
    {
        public static Action<Tower> OnTowerClicked;
    }
    
}
