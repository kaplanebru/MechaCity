using System.Collections.Generic;
using UnityEngine;

namespace UX
{
    public class CurvePointCreator
    {
        //değişken
        private Vector3 _start;
        private Vector3 _end;
        private CurveDirection curveDirection;

        //sabit
        private float _edgeDistance;
        private float _pointGap;
        private float _heightOffset;

        private Vector3 curveTangent;
        private Vector3 middle;


        public CurvePointCreator(float edgeDistance, float pointGap, float heightOffset)
        {
            _edgeDistance = edgeDistance;
            _pointGap = pointGap;
            _heightOffset = heightOffset;
        }

        public void Setup(Vector3 start, Vector3 end)
        {
            _start = start;
            _end = end;
            curveDirection = CalculateDirection();
        }

        CurveDirection CalculateDirection()
        {
            //TODO
            return CurveDirection.Left;
        }

        private void SetCurveEdge()
        {
            middle = (_start + _end) / 2;
            float pole = curveDirection == CurveDirection.Right ? -1 : 1;
            Vector3 direction = middle.normalized * pole;
            curveTangent = middle + direction * _edgeDistance;
        }

        
        private Vector3 GetPoint(float t)
        {
            SetCurveEdge();

            var lerp1 = Vector3.Lerp(_start, curveTangent, t);
            var lerp2 = Vector3.Lerp(curveTangent, _end, t);
            return Vector3.Lerp(lerp1, lerp2, t) - Vector3.up * _heightOffset;
        }

        public IEnumerable<Vector3> GetCurvePoints()
        {
            float t = 0;
            while (t <= 1)
            {
                yield return GetPoint(t); //points.Add(GetCurvePoint(gap));
                t += _pointGap;
            }
        }
    }
}