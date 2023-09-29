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
    public int _amountCheck;
    private int Amount => amount - amount % 6;

    public Transform cube;
    public Transform center;

    [ReadOnly] public List<Vector3> chainPoints = new();


    private void Start()
    {
        GetCirclePoints();
        DrawLines();
    }

    private void OnEnable()
    {
        _amountCheck = amount;
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

    void DrawLines()
    {
        lr.positionCount = Amount + 1;
        chainPoints.Add(chainPoints[0]);
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
}