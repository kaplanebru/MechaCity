using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataModels
{
    [CreateAssetMenu(fileName = nameof(BPHolder))]
    public class BPHolder : ScriptableObject
    {
        public BlueprintData[] BPData;
    }
}

