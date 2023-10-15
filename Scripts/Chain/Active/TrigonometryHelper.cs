using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEngine;

public static class TrigonometryHelper
{
    public static Vector3 CirclePoint(float angle, float radius)
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);

        return new Vector3(x, 0, y) * radius;
    }

    public static Vector3[] CommonTangentPoints(Vector3 posA, Vector3 posB, float radiusA, float radiusB, float unitOffset)
    {
        Vector3[] tangentPoints = new Vector3[2];

        var distanceZ = Mathf.Abs(posA.z - posB.z);
        var distanceX = Mathf.Abs(posB.x - posA.x);
        var dir = (posB - posA).normalized;
        var extraAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        
        //Debug.Log(distanceX);
        var distance = Vector3.Distance(posA, posB);
        //Debug.Log(distance);

        //var extraAngle = Mathf.Atan(distance) * Mathf.Rad2Deg;
        Debug.Log(extraAngle);
        
        
        var theta = (distance * radiusB) / (radiusA - radiusB);


        var angle = AngleByCos(radiusB, theta);//Mathf.Acos(radiusB / theta) * Mathf.Rad2Deg; //use same angle since they are similar triangles
        tangentPoints[1] = CirclePoint(angle + extraAngle, radiusB + unitOffset) + posB;
        tangentPoints[0] = CirclePoint(angle + extraAngle, radiusA + unitOffset) + posA;

        return tangentPoints;
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
    
    public static Vector3 CenterDirection(Arc[] arcParts)
    {
        Vector3 pos = Vector3.zero;
        foreach (var arcPart in arcParts)
        {
            pos += arcPart.gear.transform.position;
        }

        return pos / arcParts.Length;
    }
    
    
    public static int LinearPointAmountByDistance(Vector3 first, Vector3 last, float unit)
    {
        var distance = Vector3.Distance(last, first);
        return Mathf.RoundToInt(distance / unit) - 1;
    }

    public static float GetAngleByAllLength(float a, float b, float c)
    {
        float cosA = (b * b + c * c - a * a) / (2 * b * c);
        float angleA = Mathf.Acos(cosA) * Mathf.Rad2Deg;

        return angleA;
    }
}
