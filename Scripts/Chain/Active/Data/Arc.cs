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
        public Vector2 edgeSmoother = new Vector2(1 ,1);
        public Cogwheel gear;
        
        [Header("Not for user input")]
        public int id;
        public int relatedArcId;
        public EdgeAngles edgeAngles;
        public float baseAngle;
        public Vector3 nextPoint;
        public List<Vector3> arcPoints = new();

        public void SetRadiusByGear()
        {
            radius = gear.Data.Radius;
        }
    }

}
