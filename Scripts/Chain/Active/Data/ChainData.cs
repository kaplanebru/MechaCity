using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [CreateAssetMenu(fileName = nameof(ChainData))]
    public class ChainData : ScriptableObject
    {
        public ChainEnums.ChainType Type = ChainEnums.ChainType.BikeChain;
        public ChainEnums.UpAxis UpwardsAxis;
        public float Unit = 2.3f;
        public float RadiusOffset = 0.5f;
        public float Tension = 0f; //Linear Offset
        public ChainLink linkPrefab;
        public LinksPool LinksPoolPrefab;

        public float MachinerySpeed = 10;
        public float SpeedMultiplier = 0.1f;
        public float LinkRotationMultiplier = 1f;
        public ChainEnums.ChainDirection motionDirection = ChainEnums.ChainDirection.Clockwise;
        
        public bool SetRadiusByGear = true;
        public bool SetMotionByGear = true;
        public bool IsMoving = true;
        public bool FollowGearRotation = true;
        public bool LinkRotationEffect;

        [HideInInspector]public int CogAmount;
    }

   
}

