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
        public int Id;
    }

    public class TowerMover : MonoBehaviour, ITowerSegment
    {
        public TowerMoverData Data;
        public CombatTimingData timingData;
        private Rotater rotater;
        private ShakeEffect shaker;

        [Header("Shake")] public float shakeMagnitude = 0.03f;


        public void SetId(int id)
        {
            Data.Id = id;
        }

        public void Initialize()
        {
            rotater = new Rotater(Data.Middle.transform);
            shaker = new ShakeEffect(new ShakeData(
                Data.Middle.transform,
                timingData.shakeDuration,
                shakeMagnitude));
        }

        public void ChangeHeight(float newHeight)
        {
            newHeight *= Data.HeightOffset;
            Data.Middle.transform.DOScaleY(newHeight, 1).OnComplete(() =>
            {
                UIEventbus.OnTowerHeightChange?.Invoke(newHeight / Data.HeightOffset, Data.Id);
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

            UIEventbus.OnTowerHeightChange?.Invoke(newHeight / Data.HeightOffset, Data.Id);
        }

        public void Shake()
        {
            StartCoroutine(shaker.ShakeCoroutine());
        }


        public void RotateMiddle()
        {
            rotater.Rotate(360);
        }
        
    }
}