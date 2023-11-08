using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [Serializable]
    public class Arc
    {
        [HideInInspector] public float radius;
        public Cogwheel cog;

        [Header("Not for user input")] public int id;
        public int relatedArcId;
        public EdgeAngles edgeAngles;
        public float baseAngle;
        public Vector3 nextArcPoint;
        public List<Vector3> arcPoints = new();

        public Arc(Cogwheel _cog)
        {
            cog = _cog;
        }

        public void SetRadiusByGear(float ArcOffset)
        {
            radius = cog.Data.Radius + ArcOffset;
        }
    }
}