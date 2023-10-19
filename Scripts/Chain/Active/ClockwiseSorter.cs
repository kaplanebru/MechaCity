using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chain
{
    public class ClockwiseSorter
    {
        struct PointsAngles
        {
            public Vector3 Point;
            public float Angle;

            public PointsAngles(Vector3 point, float angle)
            {
                Point = point;
                Angle = angle;
            }
        }

        public Vector3[] points;
        List<float> angles = new ();
        List<PointsAngles> pointsAngles = new ();

        private Vector3 center;

        void Sort()
        {
            GetCenter();
            CalculateAngles();
            SortPointsByAngles();
            RelatePointsToAngles();
            //return points or arcs
        }

        void GetCenter()
        {
            center = TrigonometryHelper.Center(points);
        }

        void CalculateAngles()
        {
            foreach (Vector2 point in points)
            {
                float angle = Mathf.Atan2(point.y - center.y, point.x - center.x) * Mathf.Rad2Deg; //TODO: if z
                angles.Add(angle);
            }
        }

        void RelatePointsToAngles()
        {
            for (int i = 0; i < points.Length; i++)
            {
                pointsAngles.Add(new PointsAngles(points[i], angles[i]));
            }
        }

        void SortPointsByAngles()
        {
            pointsAngles = pointsAngles.OrderBy(p => p.Angle).ToList();
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = pointsAngles[i].Point;
            }
            //sort sth by anothers order
            //sort arcs here not points
            //içeri arcları al
        }
    }
}