using System;
using DG.Tweening;
using GameUI;
using UnityEngine;

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
    }
}