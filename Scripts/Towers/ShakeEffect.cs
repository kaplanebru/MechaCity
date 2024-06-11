using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Towers
{
    public class ShakeData
    {
        public Transform Transform;
        public float Duration;
        public float Magnitude;

        public ShakeData(Transform transform, float duration, float magnitude)
        {
            Transform = transform;
            Duration = duration;
            Magnitude = magnitude;
        }
    }
    public class ShakeEffect: IEnumeratorContainer
    {
        private ShakeData Data;
        public ShakeEffect(ShakeData data)
        {
            Data = data;
        }
        
        public IEnumerator LeCoroutine()
        {
            Debug.Log("shake");
            Vector3 originalPosition = Data.Transform.localPosition;
            float elapsed = 0.0f;

            while (elapsed < Data.Duration)
            {
                float x = originalPosition.x + Random.Range(-1f, 1f) * Data.Magnitude;
                float y = originalPosition.y + Random.Range(-1f, 1f) * Data.Magnitude;
                float z = originalPosition.z + Random.Range(-1f, 1f) * Data.Magnitude;

                Data.Transform.localPosition = new Vector3(x, y, z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            Data.Transform.localPosition = originalPosition;
        }
    }

}
