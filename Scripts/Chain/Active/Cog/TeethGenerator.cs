using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Chain
{
    public class TeethGenerator : MonoBehaviour
    {
        public CogData Data;
        public Transform toothPb;
        public float intervalAngle = 60;
        public float minIntervalLimit = 6;
        public bool scale = false;
        public bool equalize = false;

        private int teethCount;

        private void OnEnable()
        {
            ChainEvents.OnCogStart += CreateTeethPoints;
        }

        void SetIntervalAngle()
        {
            intervalAngle /= (Data.Radius);
            
            if(equalize)
                intervalAngle = TrigonometryHelper.Angle360(intervalAngle);
            
            if (intervalAngle < minIntervalLimit)
                intervalAngle = minIntervalLimit;
        }
        
        void CreateTeethPoints(CogData data, Transform parent)
        {
            if (parent.position != transform.position) return;
            Data = data;
            
            
            SetIntervalAngle();
            for (float i = 0; i < 360; i+=intervalAngle)
            {
                teethCount++;
                Vector3 point = TrigonometryHelper.CirclePoint(i, Data.Radius);
                Transform tooth = Instantiate(toothPb, point + transform.position, Quaternion.identity);
                tooth.SetParent(parent);
                SetTooth(tooth, point);
            }
            
            ChainEvents.OnTeethCreated?.Invoke(teethCount, transform);
        }

        void SetTooth(Transform tooth, Vector3 direction)
        {
            tooth.transform.rotation = Quaternion.LookRotation(direction);

            if (scale)
            {
                Vector3 scale = tooth.transform.localScale;
                scale.x *= Data.Radius;
                scale.z *= Data.Radius;
                tooth.transform.localScale = scale;
            }
            
            
           
        }

        private void OnDisable()
        {
            ChainEvents.OnCogStart -= CreateTeethPoints;
        }
    }
}