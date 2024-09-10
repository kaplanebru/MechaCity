using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataModels
{
    [CreateAssetMenu(fileName = nameof(CombatTimingData))]
    public class CombatTimingData : ScriptableObject
    {
        public float ProjectileDuration = 1;
        public float shooterMotionDuration = .5f;
        public float skipDelay = 0.3f;
        public float cameraDelay = 1;
        public float shakeDuration = .2f;
        public float colorFadeDuration = 1;

        public float accelerant = 10;

        public void AccelerateValues()
        {
            ProjectileDuration /= accelerant*3;
            shooterMotionDuration /= accelerant*3;
            skipDelay /= accelerant;
            cameraDelay /= accelerant;
            shakeDuration /= accelerant;
            colorFadeDuration /= accelerant;
        }
    }
}