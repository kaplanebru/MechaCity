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
        public float circularThickness = 2f;
        public ChainEnums.HoleType HoleType;
        public bool IsMoving = true;
        public bool IsChainRelated;
        

        [Header("Teeth Settings")]
        public Vector3 toothScale = Vector3.one;
        public float ToothGap = 60;
        public float MinGapLimit = 6;
        public bool Equalize = false; //TODO: POSSİBLE BUG
        [HideInInspector] public int TeethCount;
        [HideInInspector] public float ToothUnit;

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

