using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHelper
{
    public static  float GetRandomAngle()
    {
        return Random.Range(0, 360);
    }

    public static Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(GetRandomAngle(), GetRandomAngle(), GetRandomAngle());
    }
}
