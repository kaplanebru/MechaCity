using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [Serializable]
    public class Arc
    {
        public float radius;
        public float edgeSmoother = 1;
        public Cogwheel gear;
        
        [Header("Not for user input")]
        public int id;
        public int relatedArcId;
        public EdgeAngles edgeAngles;
        public float baseAngle;
        public Vector3 connectionPoint;
        public List<Vector3> arcPoints = new();

        public void SetRadiusByGear()
        {
            radius = gear.Data.Radius;
        }
    }

}
