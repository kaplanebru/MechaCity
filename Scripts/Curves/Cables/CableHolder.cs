using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Curves;
using UnityEngine;

[Serializable]
public class CableData
{
    public Transform TowerTransform;
    public Transform CurveTangent;
    public LineRenderer LineRenderer;
}
public class CableHolder : MonoBehaviour
{
    public CableData[] data;
    public Transform center;
    public float pointGap;

    private CurvePointCreator pointCreator;
 

    private void OnEnable()
    {
        pointCreator = new CurvePointCreator(pointGap);
        //GeneralEventbus.InitializerEvents.OnTowersCreated += CreateCables;
    }

    private void Start()
    {
        CreateCables();
    }

    private void CreateCables()
    {
        foreach (var structure in data)
        {
            // pointCreator.GetCurveTangentFromOutside(structure.CurveTangent.position);
            // var points = pointCreator.GetCurvePoints(
            //     structure.TowerTransform.position,
            //     center.position,
            //     false).ToArray();
            //
            // LineCreator lineCreator = new LineCreator(structure.LineRenderer);
            // lineCreator.PointsToLines(0, points);
        }
    }

    private void OnDisable()
    {
        
    }
}
