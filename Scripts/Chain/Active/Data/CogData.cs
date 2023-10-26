using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(CogData))]
    public class CogData : ScriptableObject
    {
        public string uniqueID;
        public float Radius = 4;
        public int RotationDirection = 1;
        public bool IsMoving = true;
        public float circularThickness = 2f;

        [Header("Teeth Settings")]
        public Vector3 toothScale = Vector3.one;
        public float ToothGap = 60;
        public float MinGapLimit = 6;
        public bool Equalize = false; //TODO: POSSİBLE BUG

        public void SetUniqID()
        {
            string uniqueID = Guid.NewGuid().ToString();
        }

        private void OnEnable()
        {
            SetUniqID();
        }

        //public Color Color = Color.cyan;
        //public Vector3 PositionOffset;
    }
}

