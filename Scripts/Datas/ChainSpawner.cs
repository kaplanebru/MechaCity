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
    public int straightAmount = 15;
    
    public Vector3[] curveEdges;
    public Vector3 lineEdge;
    
    [ReadOnly]public List<Vector3> chainCurvePoints = new();
    [ReadOnly]public List<Vector3> chainStraightPoints = new();

    private void Start()
    {
        GeneratePoints(curveAmount, LineType.Curved);
        GeneratePoints(curveAmount, LineType.Straight);
        lr.positionCount = straightAmount; //+curveAmount;
        DrawStraightLine();
        //DrawChain();
        
    }

    void DrawChain()
    {
        
        
        
        for (int i = straightAmount; i < lr.positionCount; i++)
        {
            lr.SetPosition(i, chainCurvePoints[i]);
        }
    }

    void DrawStraightLine()
    {
        for (int i = 0; i < lr.positionCount; i++)  //straightCount
        {
            lr.SetPosition(i, chainStraightPoints[i]);
        }
    }

    Vector3 CurvePoint(float t)
    {
        Vector3 AB = Vector3.Lerp(curveEdges[0], curveEdges[1], t);
        Vector3 BC = Vector3.Lerp(curveEdges[1], curveEdges[2], t);
        return Vector3.Lerp(AB, BC, t);
    }
    
    Vector3 StraightPoint(float t)
    {
        return Vector3.Lerp(lineEdge, curveEdges[0], t);
    }
    
    void GeneratePoints(float _amount, LineType lineType)
    {
        float ratio = 1f / _amount;
        float t = 0;
        
        switch (lineType)
        {
            case LineType.Straight:
                while (t < 1) 
                {
                    t = Mathf.MoveTowards(t, 1, ratio);
                    chainStraightPoints.Add(StraightPoint(t));
                }
                break;
            case LineType.Curved:
                while (t < 1) 
                {
                    t = Mathf.MoveTowards(t, 1, ratio);
                    chainCurvePoints.Add(CurvePoint(t));
                }
                break;
        }
        
    }

    

}