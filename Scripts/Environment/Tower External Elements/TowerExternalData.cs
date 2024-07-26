using System;
using UnityEngine;

namespace TowerExternal
{
    [Serializable]
    public class TowerExternalData
    {
        public Cable[] Cables;
        public Floor[] Floors;
        public GearIdentifier[] GearIdentifiers;
        
        public Color CableSelectionColor;
        public Color CableDefaultColor;
    }

    public interface ITowerExternal
    {
        
    }
}

