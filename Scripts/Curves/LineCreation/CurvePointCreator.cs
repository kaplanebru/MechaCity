using System.Collections.Generic;
using UnityEngine;

namespace Curves
{
    public class CurvePointCreator
    {
        //değişken
        private Vector3 _start;
        private Vector3 _end;
        
        //sabit
        private float _pointGap;
        
        //tangent calculation elements:
        private float _edgeDistance;
        private float _heightOffset;
        private CurveDirection curveDirection;

        private Vector3 _curveTangent;
        private Vector3 middle;


        public CurvePointCreator(float pointGap)
        {
            _pointGap = pointGap;
        }
        public CurvePointCreator(float edgeDistance, float pointGap, float heightOffset)
        {
            _edgeDistance = edgeDistance;
            _pointGap = pointGap;
            _heightOffset = heightOffset;
            curveDirection = CalculateDirection();
        }

        private void SetTips(Vector3 start, Vector3 end)
        {
            _start = start;
            _end = end;
        }
        
        private Vector3 GetPoint(float t)
        {
            var lerp1 = Vector3.Lerp(_start, _curveTangent, t);
            var lerp2 = Vector3.Lerp(_curveTangent, _end, t);
            return Vector3.Lerp(lerp1, lerp2, t) - Vector3.up * _heightOffset;
        }

        public IEnumerable<Vector3> GetCurvePoints(Vector3 start, Vector3 end, bool setCurveTangent = true)
        {
            SetTips(start, end);
            
            if(setCurveTangent)
                SetCurveTangent(); //not: set tips önce yapılmalı çünkü middle hesaplanıyor
            
            float t = 0;
            while (t <= 1)
            {
                yield return GetPoint(t);
                t += _pointGap;
            }
        }

        CurveDirection CalculateDirection()
        {
            //TODO
            return CurveDirection.Left;
        }

        public void GetCurveTangentFromOutside(Vector3 curveTangent)
        {
            _curveTangent = curveTangent;
        }
        private void SetCurveTangent()
        {
            float pole = curveDirection == CurveDirection.Right ? -1 : 1; 
            //not: circle yukardan aşağı dizili olduğu için son 3 towerda +lar - oluyor,
            //o yüzden aynı direction işe yarıyor, ama circle dışı dizilimlerde bu çaışmayabilir

            middle = (_start + _end) / 2;
            Vector3 direction = middle.normalized * pole;
            _curveTangent = middle + direction * _edgeDistance;
        }

    }
}