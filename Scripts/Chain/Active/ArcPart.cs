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
        public Transform gear;
        public int relatedArcId;

        public List<Vector3> arcPoints = new();
    }

}
