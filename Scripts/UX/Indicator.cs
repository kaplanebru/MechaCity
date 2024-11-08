using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UX
{
    public class Indicator : MonoBehaviour
    {
        public LineRenderer lr;
        public float pointDistance = 0.1f;
        public float edgeDistance = 3;
        
        public Transform start;
        public Transform end;
        public CurveDirection curveDirection;

        private CurvePointCreator pointCreator = new();
        private Vector3[] linePoints ;

     
        private void Start()
        {
            pointCreator.Setup(start.position, end.position, curveDirection, edgeDistance, pointDistance);
          

        
            // float textureRepeat = pointCreator.GetCurvePoints().Count() / 2.0f; // Adjust this value based on your texture and line length
            // lr.material.mainTextureScale = new Vector2(textureRepeat, 1);
           
            CreateCurve();
            
           
        }

        void CreateCurve()
        {
            linePoints = pointCreator.GetCurvePoints().ToArray();
            lr.positionCount = linePoints.Length;
            lr.SetPositions(linePoints);
        }
    }

    public enum CurveDirection
    {
        Right, Left
    }

    public class CurvePointCreator
    {
        //değişken
        private Vector3 start;
        private Vector3 end;
        private CurveDirection curveDirection;

        //sabit
        private float _edgeDistance;
        private float _pointGap;
        
        private Vector3 curveTangent;
        private Vector3 middle;
        private List<Vector3> points = new();


        public void Setup(Vector3 startPoint, Vector3 endPoint, CurveDirection direction, float edgeDistance, float pointGap)
        {
            start = startPoint;
            end = endPoint;
            curveDirection = direction;
            _edgeDistance = edgeDistance;
            _pointGap = pointGap;
        }

        private void SetCurveEdge()
        {
            middle = (start + end) / 2;
            float pole = curveDirection == CurveDirection.Right ? 1 : -1;
            Vector3 direction = middle.normalized * pole;
            curveTangent = middle + direction * _edgeDistance;
        }

        private Vector3 GetPoint(float t)
        {
            SetCurveEdge();

            var lerp1 = Vector3.Lerp(start, curveTangent, t);
            var lerp2 = Vector3.Lerp(curveTangent, end, t);
            return Vector3.Lerp(lerp1, lerp2, t);
        }

        public IEnumerable<Vector3> GetCurvePoints()
        {
            float t = 0;
            while (t<1)
            {
                t += _pointGap;
                yield return GetPoint(t); //points.Add(GetCurvePoint(gap));
            }
        }

    }

}
