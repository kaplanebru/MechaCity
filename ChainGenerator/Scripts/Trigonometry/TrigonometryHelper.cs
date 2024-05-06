using Chain;
using UnityEngine;

namespace Chain
{
    public static class TrigonometryHelper
    {
        public static Vector3 CirclePoint(float angle, float radius)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);

            Vector3 point = new Vector3(x, 0, y) * radius;

            return point;
        }


        public static float AngleInCirclePoint(Vector3 point, Vector3 referencePoint)
        {
            float angle = Mathf.Atan2(point.z - referencePoint.z, point.x - referencePoint.x) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;
            return angle;
        }

        #region Later

        // public static float AngleInPoint(Vector3 point, Vector3? referencePoint = null)
        // {
        //     if (!referencePoint.HasValue)
        //         referencePoint = Vector3.zero;
        //     
        //    
        //  
        //     float angle = Mathf.Atan2(point.z - referencePoint.z, point.x - referencePoint.Value.x) * Mathf.Rad2Deg;
        //     angle = (angle + 360) % 360;
        //     return angle;
        // }

        #endregion

        public static Vector3[]
            CommonTangentPoints(Vector3 posA, Vector3 posB, float radiusA, float radiusB) //, float offset)
        {
            Vector3 direction = (posB - posA).normalized;
            float rotationAngle = AngleInCirclePoint(direction, Vector3.zero); //AngleInPoint2(direction);

            // if (posB.x < posA.x)
            // {
            //     //(posA, posB) = (posB, posA);
            //     //rotationAngle += 180;
            // }


            float distance = Vector3.Distance(posA, posB);
            float similarHyp = (distance * radiusB) / (radiusA - radiusB);


            float angle = AngleByCos(radiusB, similarHyp); //use same angle since they are similar triangles
            angle = (angle + rotationAngle) % 360;

            Vector3[] tangentPoints = new Vector3[2];
            tangentPoints[1] = CirclePoint(angle, radiusB) + posB; //CirclePoint(angle, radiusB + offset) + posB;
            tangentPoints[0] = CirclePoint(angle, radiusA) + posA; //CirclePoint(angle, radiusA + offset) + posA;
            return tangentPoints;
        }


        public static float Angle360(float baseAngle)
        {
            var intAngle = Mathf.RoundToInt(baseAngle);
            int rest = intAngle % 6;
            return rest / 2 < 2 ? intAngle - rest : intAngle + 6 - rest;
        }

        public static float AngleBySin(float sin, float radius)
        {
            var baseAngle = Mathf.Asin(sin / radius) * Mathf.Rad2Deg;

            //var intAngle = Mathf.RoundToInt(baseAngle);
            // int rest = intAngle % 6;
            // return rest / 2 < 2 ? intAngle - rest : intAngle + 6 - rest;
            return baseAngle;
        }

        public static float AngleByCos(float cos, float radius)
        {
            return Mathf.Acos(cos / radius) * Mathf.Rad2Deg;
        }

        public static Vector3 Center2(Arc[] arcParts)
        {
            Vector3 pos = Vector3.zero;
            foreach (var arcPart in arcParts)
            {
                pos += arcPart.cog.transform.position;
            }

            return pos / arcParts.Length;
        }

        public static Vector3 Center(Vector3[] points)
        {
            Vector3 pos = Vector3.zero;
            foreach (var point in points)
            {
                pos += point;
            }

            return pos / points.Length;
        }


        public static int LinearPointAmountByDistance(Vector3 first, Vector3 last, float unit)
        {
            var distance = Vector3.Distance(last, first);
            return Mathf.RoundToInt(distance / unit) - 1;
        }

        public static float Mod360(float angle) => (angle + 360) % 360;

        public static float GetAngleByAllLength(float a, float b, float c)
        {
            float cosA = (b * b + c * c - a * a) / (2 * b * c);
            float angleA = Mathf.Acos(cosA) * Mathf.Rad2Deg;

            return angleA;
        }

        // public static void CommonIntersectionPoint(int i)
        // {
        //     Arc relatedArc = arcs[arcs[i].relatedArcId];
        //     var posA = arcs[i].gear.transform.position;
        //     var posB = relatedArc.gear.transform.position;
        //     float distance = Vector3.Distance(posA, posB);
        //
        //     Vector3 pointA = new Vector3();
        //     pointA.x = posA.x + (arcs[i].radius * (posB.x - posA.x)) / distance;
        //     pointA.z = posA.z + (arcs[i].radius * (posB.z - posA.z)) / distance;
        //
        //
        //     Vector3 pointB = new Vector3();
        //     pointB.x = posB.x + (relatedArc.radius * (posA.x - posB.x)) / distance;
        //     pointB.z = posB.z + (relatedArc.radius * (posA.z - posB.z)) / distance;
        //
        //
        //     Instantiate(testCubePb, pointB, Quaternion.identity);
        //     Instantiate(testCubePb, pointA, Quaternion.identity);
        //
        //     // P1_x = A_x + (r1 * (B_x - A_x)) / d
        //     // P1_y = A_y + (r1 * (B_y - A_y)) / d
        //
        //     // P2_x = B_x + (r2 * (A_x - B_x)) / d
        //     // P2_y = B_y + (r2 * (A_y - B_y)) / d
        // }
    }
}