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
        public ChainEnums.CogContactType ContactType; //Todo: direction set edilen yer
        public int RotationDirection = 1;
        public float HoleSize = 2f;
        public ChainEnums.HoleType HoleType;
        public bool IsMoving = true;
        public TeethPool TeethPoolPrefab;

        public Cogwheel RelatedCog; //If CONTACT TYPE IS COG RELATED
        

        [Header("Teeth Settings")]
        public Vector3 toothScale = Vector3.one;
        public float ToothGap = 60;
        public float MinGapLimit = 6;
        public bool Equalize = false; //TODO: POSSİBLE BUG
        [HideInInspector] public int TeethCount;
        [HideInInspector] public float ToothUnit;

       

        //public Color Color = Color.cyan;
        //public Vector3 PositionOffset;
    }
}

