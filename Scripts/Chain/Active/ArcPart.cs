using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [Serializable]
    public class ArcPart
    {
        public int id;
        public float radius;
        public float edgeSmoother = 1;
        public Transform gear;
        
        [Header("Not for user input")]
        public int relatedArcId;
        public Vector3 connectionPoint;
        public List<Vector3> arcPoints = new();
    }

}
