using System;
using System.Collections;
using System.Collections.Generic;
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
        public Transform Body;
        public Transform Top;
        public Transform Middle;
        public float TopOffset = 0;
        
        public float ShakeMagnitude = 0.03f;
        public CombatTimingData TimingData;
        public CommonData CommonData;

        public RiseFallData RiseFallData;
    }

    public class TowerMover : ITowerSegment
    {
        public TowerMoverData Data;
        private Rotater rotater;
        private ShakeEffect shaker;
        public RiseFallMotion riseFallMotion;
        public int Id { get; set; }

        public TowerMover(TowerSegmentData data)
        {
            Data = data as TowerMoverData;
            riseFallMotion = new RiseFallMotion(Data.RiseFallData);
            Subscribe();
        }

        public void Subscribe()
        {
            GeneralEventbus.InitializerEvents.OnOrienterReady += OrientVersTarget;
        }

        public void Unsubscribe()
        {
            GeneralEventbus.InitializerEvents.OnOrienterReady -= OrientVersTarget;
        }
        
        public void SetId(int id)
        {
            Id = id;
            riseFallMotion.SetId(Id);
        }

        public void Initialize()
        {
            rotater = new Rotater(Data.Middle.transform);
            shaker = new ShakeEffect(new ShakeData(
                Data.Middle.transform,
                Data.TimingData.shakeDuration,
                Data.ShakeMagnitude));

        }

        public void OrientVersTarget(Vector3 target)
        {
            var newRot = Quaternion.LookRotation(target-Data.Body.position) * Quaternion.Euler(0, 180 ,0);
            var towerRot = Data.Body.eulerAngles;
          
            Data.Body.rotation = Quaternion.Euler(towerRot.x, newRot.eulerAngles.y, towerRot.z);
        }

        public void SetHeightPhysically(float newHeight, bool isRising)
        {
            newHeight *= Data.CommonData.TowerHeightPerStep;
            riseFallMotion.UpdateData(newHeight, isRising);
            
            //DOTween.KillAll(Data.Top.transform.gameObject); //for earthquake
            Data.Top.transform.DOLocalMoveY(newHeight + Data.TopOffset, 1);
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