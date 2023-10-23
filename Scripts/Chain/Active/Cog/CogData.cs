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
        public float Radius = 4;
        
        public int RotationDirection = 1;
        //public Color Color = Color.cyan;
        //public Vector3 PositionOffset;
        public bool IsMoving = true;
        public Vector3 toothScale = Vector3.one;
        public float circularThickness = 2f;
        
        public Transform[] holes;
        public Transform cogObject;
    }
}

