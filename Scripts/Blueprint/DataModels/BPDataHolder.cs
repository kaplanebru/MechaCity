using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataModels
{
    [CreateAssetMenu(fileName = nameof(BPDataHolder))]
    public class BPDataHolder : ScriptableObject
    {
        public BlueprintData[] BPData;
    }
}

