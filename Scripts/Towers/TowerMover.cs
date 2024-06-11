using System;
using System.Collections;
using DataModels;
using DG.Tweening;
using GameUI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Towers
{
    [Serializable]
    public class TowerMoverData
    {
        public Transform Top;
        public Transform Middle;
        public float TopOffset = 0;
        public float HeightOffset = 1.5f;
    }

    public class TowerMover : MonoBehaviour
    {
        public TowerMoverData Data;
        public CombatTimingData timingData;
        private Rotater rotater;

        [Header("Shake")] float shakeMagnitude = 0.03f;
        
        public void Initialize()
        {
            rotater = new Rotater(Data.Middle.transform);
        }

       
        public void ChangeHeight(float newHeight)
        {
            newHeight *= Data.HeightOffset;
            Data.Middle.transform.DOScaleY(newHeight, 1).OnComplete(() =>
            {
                UIEventbus.OnTowerHeightChange?.Invoke(newHeight/Data.HeightOffset, gameObject);
            });

            Data.Top.transform.DOLocalMoveY(newHeight + Data.TopOffset, 1);
        }
        
        public void SetHeight(float newHeight)
        {
            newHeight *= Data.HeightOffset;
            
            var scale = Data.Middle.transform.localScale;
            var pos = Data.Top.transform.localPosition;
            
            scale.y = newHeight;
            pos.y = newHeight;
            
            Data.Middle.transform.localScale = scale;
            Data.Top.transform.localPosition = pos;
            
            UIEventbus.OnTowerHeightChange?.Invoke(newHeight/Data.HeightOffset, gameObject);
        }

        public void Shake()
        {
            StartCoroutine(ShakeCoroutine(Data.Middle.transform));
        }

        private IEnumerator ShakeCoroutine(Transform middleTransform)
        {
            Vector3 originalPosition = middleTransform.localPosition;
            float elapsed = 0.0f;

            while (elapsed < timingData.shakeDuration)
            {
                float x = originalPosition.x + Random.Range(-1f, 1f) * shakeMagnitude;
                float y = originalPosition.y + Random.Range(-1f, 1f) * shakeMagnitude;
                float z = originalPosition.z + Random.Range(-1f, 1f) * shakeMagnitude;

                middleTransform.localPosition = new Vector3(x, y, z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            middleTransform.localPosition = originalPosition;
        }
        
        public void RotateMiddle()
        {
           rotater.Rotate(360);
        }
    }
}