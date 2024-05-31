using System;
using System.Collections;
using DG.Tweening;
using GameUI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Towers
{
    [Serializable]
    public class TowerPartsData
    {
        public Transform Top;
        public Transform Middle;
        public Transform Down;
        public MeshRenderer[] MiddleMeshes;
        public MeshRenderer TopMesh;
    }

    public class TowerParts : MonoBehaviour
    {
        public TowerPartsData Data;
        
        [Header("Shake")]
         float duration = .2f;
         float magnitude = 0.03f;



        public void SetColor(Material[] mats)
        {
            Data.MiddleMeshes[0].material = mats[0];
            for (var i = 1; i < Data.MiddleMeshes.Length; i++)
            {
                var mesh = Data.MiddleMeshes[i];
                mesh.material = mats[1];
            }
        }

        public void ChangeHeight(float newHeight)
        {
            Data.Middle.transform.DOScaleY(newHeight, 1).OnComplete(() =>
            {
                UIEventbus.OnTowerHeightChange?.Invoke(newHeight, gameObject);
            });

            Data.Top.transform.DOLocalMoveY(newHeight, 1); //newHeight + 1 de olur
            //down rotate
        }

        public void Shake()
        {
            StartCoroutine(ShakeCoroutine(Data.Middle.transform));
        }
        
        private IEnumerator ShakeCoroutine(Transform middleTransform)
        {
            Vector3 originalPosition = middleTransform.localPosition;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = originalPosition.x + Random.Range(-1f, 1f) * magnitude;
                float y = originalPosition.y + Random.Range(-1f, 1f) * magnitude;
                float z = originalPosition.z + Random.Range(-1f, 1f) * magnitude;

                middleTransform.localPosition = new Vector3(x, y, z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            middleTransform.localPosition = originalPosition;
        }
    }
}