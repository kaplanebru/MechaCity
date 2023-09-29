using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ChainSpawner2 : MonoBehaviour
{
    public LineRenderer lr;
    Material lrMat;
    public float radius = 1;

    public int amount;
    private int _amountCheck;
    private int Amount => amount - amount % 6;

    public Transform cube;
    public Transform center;

    [ReadOnly] public List<Vector3> chainPoints = new();


    private void Start()
    {
        GetCirclePoints();
        SplitCircle();
        DrawLines();
    }

    private void OnEnable()
    {
        _amountCheck = amount;
    }

    void SplitCircle()
    {
        for (int i = Amount/2+1; i < Amount; i++)
        {
            var pos = chainPoints[i];
            pos.z -= 10;
            chainPoints[i] = pos;
        }
    }

    void DrawLines()
    {
         lr.positionCount = Amount + 1;
         chainPoints.Add(chainPoints[0]);

        //lr.positionCount = Amount / 2+1;
        lr.SetPositions(chainPoints.ToArray());
    }

    void GetCirclePoints()
    {
        float baseAngle = 360f / Amount;
        for (int i = 0; i < Amount; i++)
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
    
    void InstaintiateCubes()
    {
        for (int i = 0; i < Amount; i++)
        {
            Instantiate(cube, chainPoints[i], Quaternion.identity);
        }
    }
    
    private void OnValidate()
    {
        if (_amountCheck != amount)
        {
            _amountCheck = amount;
            lr.positionCount = 0;
            chainPoints.Clear();
            GetCirclePoints();
            DrawLines();
        }
    }
}