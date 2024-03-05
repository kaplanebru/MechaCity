using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataModels
{
    [CreateAssetMenu(fileName = nameof(CombatTimingData))]
    public class CombatTimingData : ScriptableObject
    {
        public float shootDuration = 1;
        public float skipDelay = 0.3f;
        public float cameraDelay = 1;
    }
}