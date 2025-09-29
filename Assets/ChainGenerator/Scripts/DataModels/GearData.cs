using System;
using UnityEngine;

namespace Chain
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(GearData), menuName = "Chain Generator/" + nameof(GearData))]
    public class GearData : ScriptableObject, IMachinePartData
    {
        public float Radius = 4;
        public float Volume = 4f;
        public float HoleDepth = 2f;
        public ChainEnums.CogContactType ContactType; //Todo: direction set edilen yer
        public int RotationDirection = 1;
        public float HoleSize = 3f;
        public int HoleId;
        public bool IsMoving = true;
        public bool WithoutTeeth = false;
        public TeethPool TeethPoolPrefab;
        public GearData relatedGearData; //If CONTACT TYPE IS COG RELATED


        [Header("Teeth Settings")] 
        public Vector3 toothScale = new Vector3(.6f, .5f, .5f);
        public float ToothGap = 45;
        public float MinGapLimit = 6;
        public bool Equalize = false; //TODO: POSSİBLE BUG
        [HideInInspector] public int TeethCount;
        [HideInInspector] public float ToothUnit;
    }
}

