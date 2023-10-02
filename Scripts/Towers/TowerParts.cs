using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using UnityEngine;
using Clicks;

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


        public void SetColor(Material mat)
        {
            foreach (var mesh in Data.MiddleMeshes)
            {
                mesh.material = mat;
            }
        }

        public void ChangeHeight(float newHeight)
        {
            Data.Middle.transform.DOScaleY(newHeight, 1).OnComplete(() =>
            {
                Eventbus.UIEvents.OnTowerHeightChange?.Invoke(newHeight, gameObject);
            });

            Data.Top.transform.DOLocalMoveY(newHeight, 1);
            //down rotate
        }
    }
}