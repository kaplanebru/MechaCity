using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ChainSpawner : MonoBehaviour
{
    public LineRenderer lr;
    public int amount = 15;
    public Vector3[] points;
    [ReadOnly]public List<Vector3> chainPoints = new();

    private void Start()
    {
        SetChainPoints(amount);
        
        lr.positionCount = amount;
        for (int i = 0; i < amount; i++)
        {
            lr.SetPosition(i, chainPoints[i]);
        }
        
    }

    Vector3 ChainPoint(float t)
    {
        Vector3 AB = Vector3.Lerp(points[0], points[1], t);
        Vector3 BC = Vector3.Lerp(points[1], points[2], t);
        return Vector3.Lerp(AB, BC, t);
    }
    
    public void SetChainPoints(float _amount)
    {
        float ratio = 1f / _amount;
        float t = 0;
        while (t < 1) 
        {
            t = Mathf.MoveTowards(t, 1, ratio);
            chainPoints.Add(ChainPoint(t));
        }
    }

}