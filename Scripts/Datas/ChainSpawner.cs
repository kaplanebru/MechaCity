using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum LineType
{
    Straight,
    Curved
}

public class ChainSpawner : MonoBehaviour
{
    public LineRenderer lr;

    public int curveAmount = 15;
    

    public Transform[] curveEdges;
    public Transform lineEdge;

    [ReadOnly] public List<Vector3> chainPoints = new();
   

    private void Start()
    {
        chainPoints.Clear();
        lr.positionCount = 0;
        GeneratePoints(curveAmount);
        
       DrawChain();
       CloseEdges();
        
    }

    void DrawChain()
    {
        lr.positionCount = curveAmount * 2-2;
        lr.SetPositions(chainPoints.ToArray());
    }
    
    Vector3 CurvePoint(float t)
    {
        Vector3 AB = Vector3.Lerp(curveEdges[0].position, curveEdges[1].position, t);
        Vector3 BC = Vector3.Lerp(curveEdges[1].position, curveEdges[2].position, t);
        return Vector3.Lerp(AB, BC, t);
    }

    Vector3 StraightPoint(float t)
    {
        return Vector3.Lerp(lineEdge.position, curveEdges[0].position, t);
    }

    void GeneratePoints(int _amount)
    {
        float ratio = 1f / _amount;
        float t = 0;


        int counter = 0;
        while (t < 1)
        {
            counter++;
            if(counter == _amount-1) break;
            t = Mathf.MoveTowards(t, 1, ratio);
            chainPoints.Add(StraightPoint(t));
           
        }

        t = 0;
        
        while (t < 1)
        {
            t = Mathf.MoveTowards(t, 1, ratio);

            // if (counter < 5)
            // {
            //     counter++;
            //     continue;
            // }
            chainPoints.Add(CurvePoint(t));
           // counter++;
        }
    }

    void CloseEdges()
    {
        foreach (var curveEdge in curveEdges)
        {
            curveEdge.gameObject.SetActive(false);
        }
    }
}

