using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;


namespace Grid
{
    [Serializable]
    public class Slot
    {
        public bool HasTower = true;
        public int Id;
        public Tower Tower;
    }

}
