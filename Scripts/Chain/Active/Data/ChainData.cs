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
        public float LinearSpeed = 0.1f;
        public float LinkRotationExtent = 0.06f;
        public bool SetRadiusByObject = true;
        public bool IsMoving = true;
        public ChainDirection motionDirection = ChainDirection.Clockwise;
        public float RadiusOffset = 0.5f;
        public float LinearOffset = 0.5f;
    }
}

