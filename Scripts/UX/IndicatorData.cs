using System;
using UnityEngine;

namespace UX
{
    [Serializable]
    public class IndicatorData
    {
        public float PointDistance = 0.1f;
        public float EdgeDistance = 3;
        public float HeightOffset = 2;
        
        public LineRenderer[] LineRenderers;
    }
}