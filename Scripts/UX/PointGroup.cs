using UnityEngine;

namespace UX
{
    public class PointGroup
    {
        public int Index;
        public Vector3[] Points;

        public PointGroup(int index, Vector3[] points)
        {
            Index = index;
            Points = points;
        }
    }
}