using System;
using System.Collections;
using MyNamespace;
using UnityEngine;

namespace Chain
{
    [Serializable]
    public class CogData
    {
        public float Radius = 3;
        public int RotationDirection = 1;
        public Color Color = Color.cyan;
        public Vector3 PositionOffset;
        public bool IsMoving = true;
        public Vector3 toothScale = Vector3.one;
        public float circularThickness = 0.5f;
        public Transform[] holes;
    }

   
    public class Cogwheel : MonoBehaviour
    {
        public CogData Data;
        float speed;
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

        private void SetSpeedAndMove(int teethCount, Transform _transform, float interval)
        {
            if(transform != _transform) return;
            if(!Data.IsMoving) return;

            speed = ChainMover.CogSpeed / teethCount;
            ChainEvents.OnCogSpeedSet?.Invoke(teethCount, interval);
            
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
            SetHoleSize();
            
            ChainEvents.OnCogStart?.Invoke(Data, transform);
        }

        public void SetSpinDirection(ChainEnums.ChainDirection chainDirection)
        {
            Data.RotationDirection = chainDirection == ChainEnums.ChainDirection.Clockwise ? 1 : -1;
        }

        void SetHoleSize()
        {
            var holeSize = (Data.Radius - Data.circularThickness) *2;
            foreach (var hole in Data.holes)
            {
                Vector3 inverseParentScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y, 1f / transform.localScale.z);
                Vector3 scale = hole.transform.localScale;
                scale.x = holeSize;
                scale.z = holeSize;
                hole.transform.localScale = Vector3.Scale(scale, inverseParentScale);
            }
        }

        IEnumerator SpinRoutine()
        {
            //print(speed);
            var direction = Vector3.up * Data.RotationDirection;
            while (true)
            {
                transform.Rotate(direction, speed); //Todo: enum yapılabilir, yukarı aşağı sağ sol
                // transform.rotation =
                //     Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(direction), Data.Speed);
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
