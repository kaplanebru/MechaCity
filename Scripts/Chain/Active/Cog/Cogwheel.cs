using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Chain
{
    [Serializable]
    public class CogData
    {
        public float Radius = 3;
        public float Speed = 10; //Todo: according to radius yap
        public int RotationDirection = 1;
        public Color Color = Color.cyan;
        public Vector3 PositionOffset;
        public bool IsMoving = true;

    }
    
    public class Cogwheel : MonoBehaviour
    {
        public CogData Data;

        private void OnEnable()
        {
            ChainEvents.OnTeethCreated += SetSpeedAndMove;
            ChainEvents.OnMotionStateSet += Initialize;
            //MoveCog; //TODO: bu kısım sadece moving statei set etsin. harekete başlatan başka bir etken olmalı
        }
        
        private void Initialize(bool isMoving)
        {
            Setup();
            Data.IsMoving = isMoving;
        }

        private void SetSpeedAndMove(int teethCount, Transform _transform)
        {
            if(transform != _transform) return;
            if(!Data.IsMoving) return;

            Data.Speed = ChainMover.CogSpeed / teethCount;
            print(Data.Speed);
            StartCoroutine(nameof(SpinRoutine));
        }
        

        void Setup()
        {
            var radius = Data.Radius;
            var scale = transform.localScale;
            scale.x = radius * 2;
            scale.z = radius * 2;
            transform.localScale = scale;
            transform.position += Data.PositionOffset;
            
            ChainEvents.OnCogStart?.Invoke(Data, transform);
        }

        public void SetRotationDirection(ChainDirection chainDirection)
        {
            Data.RotationDirection = chainDirection == ChainDirection.Clockwise ? 1 : -1;
        }

        IEnumerator SpinRoutine()
        {
            while (true)
            {
                transform.Rotate(Vector3.up * Data.RotationDirection, Data.Speed);
                yield return null;
            }
        }

        private void OnDisable()
        {
            ChainEvents.OnTeethCreated -= SetSpeedAndMove;
            ChainEvents.OnMotionStateSet -= Initialize;
        }
    }

}
