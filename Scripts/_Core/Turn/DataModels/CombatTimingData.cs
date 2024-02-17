using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataModels
{
    [CreateAssetMenu(fileName = nameof(CombatTimingData))]
    public class CombatTimingData : ScriptableObject
    {
        public float shootDuration = 1;
        public float afterCombatDelay = .3f;
        public float skipDelay = 0.3f;
        public float selectionDelay = 0.3f;
        public float cursorDuration = 0.5f;
        public float cameraDelay = 1;
    }
}