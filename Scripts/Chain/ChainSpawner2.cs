using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class ChainSpawner2 : MonoBehaviour
{
    public LineRenderer lr;
    Material lrMat;
    public float radius = 1;
    private float directionAngle;


    public int amount;
    private int Amount => amount - amount % 6;

    public Transform cube;
    public Transform center;
    public Transform destination;

    [ReadOnly] public List<Vector3> chainPoints = new();


    private void Start()
    {
        ResetValues();
        
        GetCirclePoints();
        DrawLines();

    }

    void ResetValues()
    {
        chainPoints.Clear();
        lr.positionCount = 0;
    }

    private Vector3 direction;

    void SetDirectionAngle()
    {
        direction = (destination.transform.position - transform.position).normalized;
        directionAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        print(directionAngle);
    }

    void SplitCircle()
    {
        for (int i = Amount / 2 + 1; i < Amount; i++)
        {
            var pos = chainPoints[i];
            //pos.z -= 10;
            //pos.z += destination.transform.position.z;
            //pos.x += destination.transform.position.x;
            //pos += direction;
            chainPoints[i] = pos;
        }
    }

    void DrawLines()
    {
        lr.positionCount = Amount + 1;
        lr.SetPositions(chainPoints.ToArray());
    }

    void GetCirclePoints()
    {
        SetDirectionAngle();
        float baseAngle = 360f / Amount; //Mathf.RoundToInt(directionAngle);

        for (int i = 1; i <= Amount; i++)
        {
            var newAngle = (baseAngle * i + directionAngle) % 360;
            chainPoints.Add(CirclePoint(newAngle));
        }
        

        chainPoints.Add(chainPoints[0]);
    }

    Vector3 CirclePoint(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);

        return new Vector3(x, 0, y) * radius; // + transform.position;
    }

    void InstaintiateCubes()
    {
        for (int i = 0; i < Amount; i++)
        {
            Instantiate(cube, chainPoints[i], Quaternion.identity);
        }
    }
}