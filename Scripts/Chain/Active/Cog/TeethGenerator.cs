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
        public float intervalAngle = 25;
        public bool scale = false;

        private void OnEnable()
        {
            ChainEvents.OnCogSet += CreateTeethPoints;
        }

        void SetIntAngle()
        {
            intervalAngle = TrigonometryHelper.Angle360(intervalAngle, Data.Radius);
        }
        
        void CreateTeethPoints(CogData data, Transform parent)
        {
            if (parent.position != transform.position) return;
            Data = data;
            intervalAngle /= (Data.Radius * 0.1f);
            SetIntAngle();
            for (float i = 0; i < 360; i+=intervalAngle)
            {
                Vector3 point = TrigonometryHelper.CirclePoint(i, Data.Radius);
                Transform tooth = Instantiate(toothPb, point + transform.position, Quaternion.identity);
                tooth.SetParent(parent);
                SetTooth(tooth, point);
            }
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
            ChainEvents.OnCogSet -= CreateTeethPoints;
        }
    }
}