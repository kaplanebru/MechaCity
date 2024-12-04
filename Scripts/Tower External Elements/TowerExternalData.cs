using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerExternal
{
    [Serializable]
    public class TowerExternalData
    {
        public Cable[] Cables;
        public Floor[] Floors;
        public IGear[] IGears;
        public Shield[] Shields;
        public MultiShooter[] MultiShooters;
        
        public Color CableSelectionColor;
        public Color CableDefaultColor;
    }

    public interface ITowerExternal
    {
        
    }
}

