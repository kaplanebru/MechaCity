using System;
using System.Collections;
using System.Collections.Generic;
using Blueprint;
using Enums;
using UnityEngine;

namespace DataModels
{
    [CreateAssetMenu(fileName = nameof(BPDataHolder))]
    public class BPDataHolder : ScriptableObject
    {
        public BpTypeDataPair[] SerializedTypeDataPair;
        public readonly Dictionary<BpType, BlueprintData> TypeDataPair = new ();

        
        private void OnEnable()
        {
            Setup();
        }
        void Setup()
        {
            foreach (var pair in SerializedTypeDataPair)
            {
                TypeDataPair.Add(pair.Type, pair.Data);
            }
        }

    }
}

