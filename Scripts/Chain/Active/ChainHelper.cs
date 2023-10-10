using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEngine;

public static class ChainHelper
{
    public static Vector3 CirclePoint(float angle, float radius)
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);

        return new Vector3(x, 0, y) * radius;
    }

    public static int AngleByDistance(float unit, float radius)
    {
        var baseAngle = Mathf.Asin(unit / radius) * Mathf.Rad2Deg;

        var intAngle = Mathf.RoundToInt(baseAngle);
        // int rest = intAngle % 6;
        // return rest / 2 < 2 ? intAngle - rest : intAngle + 6 - rest;
        return intAngle;
    }
    
    public static Vector3 CenterDirection(ArcPart[] arcParts)
    {
        Vector3 pos = Vector3.zero;
        foreach (var arcPart in arcParts)
        {
            pos += arcPart.gear.position;
        }

        return pos / arcParts.Length;
    }
    
    public static int LinearPointAmountByDistance(Vector3 first, Vector3 last, float unit)
    {
        var distance = Vector3.Distance(last, first);
        return Mathf.RoundToInt(distance / unit) - 1;
    }
}
