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
public class CableGenerator : MonoBehaviour
{
    public Transform center;
    public float pointGap;
    public CableData[] data;
    public bool closeTangentMesh = true;
    
    private CurvePointCreator pointCreator;
    
    private void OnEnable()
    {
        pointCreator = new CurvePointCreator(pointGap);
    }

    private void Start()
    {
        CreateCables();
    }

    private void CreateCables()
    {
        foreach (var structure in data)
        {
            pointCreator.GetCurveTangentFromOutside(structure.CurveTangent.position);
            var points = pointCreator.GetCurvePoints(
                structure.TowerTransform.position,
                center.position,
                false).ToArray();
            
            LineCreator lineCreator = new LineCreator(structure.LineRenderer);
            lineCreator.PointsToLines(0, points);
            
            CloseTangentMesh(structure.CurveTangent);
        }
    }

    void CloseTangentMesh(Transform tangent)
    {
        if(!closeTangentMesh) return;
        var mesh = tangent.GetComponent<MeshRenderer>();
        mesh.enabled = false;
    }
}
