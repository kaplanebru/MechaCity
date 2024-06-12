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
    public class TowerMoverData : TowerSegmentData
    {
        public Transform Top;
        public Transform Middle;
        public float TopOffset = 0;
        
        public float ShakeMagnitude = 0.03f;
        public CombatTimingData TimingData;
        public CommonData CommonData;
    }

    public class TowerMover : ITowerSegment
    {
        public TowerMoverData Data;
        private Rotater rotater;
        private ShakeEffect shaker;
        public int Id { get; set; }

        public TowerMover(TowerSegmentData data)
        {
            Data = data as TowerMoverData;
        }
        
        public void SetId(int id)
        {
            Id = id;
        }

        public void Initialize()
        {
            rotater = new Rotater(Data.Middle.transform);
            shaker = new ShakeEffect(new ShakeData(
                Data.Middle.transform,
                Data.TimingData.shakeDuration,
                Data.ShakeMagnitude));
        }

        public void ChangeHeight(float newHeight)
        {
            newHeight *= Data.CommonData.TowerHeightPerStep;
            Data.Middle.transform.DOScaleY(newHeight, 1).OnComplete(() =>
            {
                UIEventbus.OnTowerHeightChange?.Invoke(newHeight / Data.CommonData.TowerHeightPerStep, Id);
            });

            Data.Top.transform.DOLocalMoveY(newHeight + Data.TopOffset, 1);
        }

        public void SetHeight(float newHeight)
        {
            newHeight *= Data.CommonData.TowerHeightPerStep;

            var scale = Data.Middle.transform.localScale;
            var pos = Data.Top.transform.localPosition;

            scale.y = newHeight;
            pos.y = newHeight;

            Data.Middle.transform.localScale = scale;
            Data.Top.transform.localPosition = pos;

            UIEventbus.OnTowerHeightChange?.Invoke(newHeight / Data.CommonData.TowerHeightPerStep, Id);
        }

        public void Shake()
        {
            GeneralEventbus.OnCoroutineTrigger?.Invoke(shaker);
        }


        public void RotateMiddle()
        {
            rotater.Rotate(360);
        }
        
    }
}