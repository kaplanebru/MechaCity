using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Chain
{
    public class TeethGenerator
    {
        private CogData Data;
        private TeethPool _pool;
        private Transform _transform;
        private Vector3 inverseParentScale;
       

        public TeethGenerator(CogData data, TeethPool pool, Transform transform)
        {
            Data = data;
            _pool = pool;
            _transform = transform;
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

        
        public List<Tooth> CreateTeeth()
        {
            List<Tooth> teeth = new();
            GetInverseScale();

            var _intervalAngle = SetIntervalAngle();

            for (float i = 0; i < 360; i += _intervalAngle)
            {
                Vector3 point = TrigonometryHelper.CirclePoint(i, Data.Radius);

                Tooth tooth = _pool.GetItem(t =>
                {
                    t.transform.position = _transform.position + _transform.rotation * point;
                    t.transform.localScale = Vector3.Scale(Data.toothScale, inverseParentScale);
                    t.transform.localRotation = Quaternion.LookRotation(point);
                });
                
                teeth.Add(tooth);
            }

            return teeth;
        }

        void GetInverseScale()
        {
            inverseParentScale = new Vector3(1f / _transform.localScale.x, 1f / _transform.localScale.y,
                1f / _transform.localScale.z);
        }
        
        public void ReleasePreviousTeeth(List<Tooth> previousTeeth)
        {
            if (_pool == null) //for bug check, temporary
            {
                Debug.LogError("teeth pool null");
                return;
            }

            if (_pool.pool.Count == 0)
                _pool.ActivatePool();

            if (previousTeeth.Count > 0 && previousTeeth.Any(t => t == null))
                previousTeeth.Clear();

            previousTeeth.ForEach(t => _pool.ReleaseItem(t));
            previousTeeth.Clear();
        }
        

        // void SetTeethSize()
        // {
        //     GetInverseScale();
        //     teeth.ForEach(t =>t.transform.localScale = Vector3.Scale(Data.toothScale, inverseParentScale));
        // }

       

        
    }
}