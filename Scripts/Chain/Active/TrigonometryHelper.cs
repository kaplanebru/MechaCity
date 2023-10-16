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

    public static Vector3[] CommonTangentPoints(Vector3 posA, Vector3 posB, float radiusA, float radiusB, float offset)
    {
        Vector3 direction = (posB - posA).normalized;
        float rotationAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        Debug.Log(rotationAngle);
        
        var distanceX = Mathf.Abs(posB.x - posA.x);
        float distance = Vector3.Distance(posA, posB);
        
        float similarHyp = (distance * radiusB) / (radiusA - radiusB);
        float angle = AngleByCos(radiusB, similarHyp); //use same angle since they are similar triangles
        
        Vector3[] tangentPoints = new Vector3[2];
        tangentPoints[1] = CirclePoint(angle + rotationAngle, radiusB + offset) + posB;
        tangentPoints[0] = CirclePoint(angle + rotationAngle, radiusA + offset) + posA;
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
