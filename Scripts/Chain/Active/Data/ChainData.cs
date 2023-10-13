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
        public float MotionSpeed = 0.1f;
        public bool SetRadiusByObject = true;
        public bool IsMoving = true;
    }
}

