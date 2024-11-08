using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UX
{
    public class IndicatorData
    {
        public Vector3 Start;
        public Vector3[] Ends;
    }

    public class Indicator : MonoBehaviour
    {
        private IndicatorData Data = new();
        public LineRenderer lr;
        public float pointDistance = 0.1f;
        public float edgeDistance = 3;
        public float heightOffset = 2;

        public CurveDirection curveDirection; //to calculate

        private CurvePointCreator pointCreator;
        private Vector3[] linePoints;

        private void OnEnable()
        {
            pointCreator = new(edgeDistance, pointDistance, heightOffset);
            Eventbus.IndicatorEvents.OnGettingIndicatorData += ShowLines;
        }

        private void ShowLines(Vector3 start, params Vector3[] endPositions)
        {
            foreach (var end in endPositions)
            {
                CreateCurve(start, end);
            }
        }

      
        void CreateCurve(Vector3 start, Vector3 end)
        {
            pointCreator.Setup(start, end);
            
            linePoints = pointCreator.GetCurvePoints().ToArray();
            lr.positionCount = linePoints.Length;
            lr.SetPositions(linePoints);
        }
        
        private void OnDisable()
        {
            Eventbus.IndicatorEvents.OnGettingIndicatorData -= ShowLines;
        }

        #region Tiling

        // void TilingDots()
        // {
        //     Material lineMaterial = new Material(Shader.Find("Unlit/Transparent")); 
        //     lineMaterial.mainTexture = texture;
        //     lr.material = lineMaterial;
        //     float textureRepeat = lr.positionCount/3f;
        //     lr.material.mainTextureScale = new Vector2(textureRepeat, 1);
        // }

        #endregion
    }

    public enum CurveDirection
    {
        Right,
        Left
    }
}