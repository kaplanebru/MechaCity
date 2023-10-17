using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Chain
{
    [CreateAssetMenu(fileName = nameof(ChainData))]
    public class ChainData : ScriptableObject
    {
        public ChainType Type;
        
        public float Unit = 2.3f;
        public float RadiusOffset = 0.5f;
        public float Tension = -0.5f; //Linear Offset
        
        public float Speed = 0.1f;
        public float LinkRotationMultiplier = 1f;
        public ChainDirection motionDirection = ChainDirection.Clockwise;
        
        public bool SetRadiusByGear = true;
        public bool SetMotionByGear = true;
        public bool IsMoving = true;
    }
}

