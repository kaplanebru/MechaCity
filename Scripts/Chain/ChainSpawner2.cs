using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ChainSpawner2 : MonoBehaviour
{
    public LineRenderer lr;
    Material lrMat;
    public Transform center;
    public float radius = 1;
    public int amount = 10;
    public Transform cube;
    


    [ReadOnly] public List<Vector3> chainPoints = new();


    private void Start()
    {
       GetCirclePoints();
       InstaintiateCubes();
       
    }

    void InstaintiateCubes()
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(cube, chainPoints[i], Quaternion.identity);
        }
    }

    void GetCirclePoints()
    {
        amount -= 10 % 6;
        print(amount);
        
        float baseAngle = 360f / amount;
        for (int i = 0; i < amount; i++)
        {
            chainPoints.Add(CirclePoint(baseAngle * i));
        }
    }

    Vector3 CirclePoint(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);
        
        return new Vector3(x, 0, y) * radius;
    }
}