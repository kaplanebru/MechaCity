using System;
using System.Collections;
using MyNamespace;
using UnityEngine;

namespace Chain
{
    public class Cogwheel : MonoBehaviour
    {
        public CogData Data;
        public Transform cogObject;
        public Transform []holes;
        public Transform teeth;
        float speed;

        private void OnEnable()
        {
            ChainEvents.OnTeethCreated += SetSpeed;
            ChainEvents.OnMotionStateSet += Initialize;
        }

        private void Start()
        {
            ChainEvents.OnCogDataSet?.Invoke(Data, teeth);
        }


        private void Initialize(bool isMoving)
        {
            //Setup();
            Data.IsMoving = isMoving;
            if (!Data.IsMoving) return;
            StartCoroutine(nameof(SpinRoutine));
        }

        private void SetSpeed(int teethCount, Transform _transform, float interval)
        {
            if (transform != _transform) return;

            speed = ChainMover.MachinerySpeed / teethCount;
//            print(speed);
            ChainEvents.OnCogSpeedSet?.Invoke(teethCount, interval, Data.toothScale.x);
            ChainEvents.OnCogReady?.Invoke(new object[] {this});//Data, teeth //new object[] {Data, teeth}
        }
        
        public void SetSpinDirection(ChainEnums.ChainDirection chainDirection)
        {
            Data.RotationDirection = chainDirection == ChainEnums.ChainDirection.Clockwise ? 1 : -1;
        }
        
        IEnumerator SpinRoutine()
        {
            var direction = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                ? Vector3.up * Data.RotationDirection
                : -Vector3.forward * Data.RotationDirection;

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
            ChainEvents.OnTeethCreated -= SetSpeed;
            ChainEvents.OnMotionStateSet -= Initialize;
        }
    }
}