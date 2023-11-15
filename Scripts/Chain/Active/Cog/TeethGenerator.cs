using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;


namespace Chain
{
    public class TeethGenerator
    {
        private CogData Data;
        private TeethPool _pool;

        public TeethGenerator(CogData data, TeethPool pool)
        {
            Data = data;
            _pool = pool;
        }

        float SetIntervalAngle()
        {
            var _intervalAngle = Data.ToothGap / (Data.Radius);
            if (Data.Equalize)
                _intervalAngle = TrigonometryHelper.Angle360(_intervalAngle);

            if (_intervalAngle < Data.MinGapLimit)
                _intervalAngle = Data.MinGapLimit;

            return _intervalAngle;
        }

        public List<Tooth> CreateTeeth(Transform transform)
        {
            List<Tooth> teeth = new();
            
            Vector3 inverseParentScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y,
                1f / transform.localScale.z);

            var _intervalAngle = SetIntervalAngle();

            for (float i = 0; i < 360; i += _intervalAngle)
            {
                Vector3 point = TrigonometryHelper.CirclePoint(i, Data.Radius);

                Tooth tooth = _pool.GetItem(t =>
                {
                    t.transform.position = transform.position + transform.rotation * point;
                    t.transform.localScale = Vector3.Scale(Data.toothScale, inverseParentScale);
                    t.transform.localRotation = Quaternion.LookRotation(point);
                });

                teeth.Add(tooth);
            }


            return teeth;
        }

        
    }
}