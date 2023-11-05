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

        private void OnEnable()
        {
            ChainEvents.OnMotionStateSet += StartMotion;
        }

        private void Start()
        {
            StartMotion(Data.IsMoving); //after edit
        }


        private void StartMotion(bool isMoving)
        {
            Data.IsMoving = isMoving;
            if (!Data.IsMoving) return;
            
            SetSpeedByTeeth();
            ChainEvents.OnCogSpeedSet?.Invoke(Data.TeethCount, Data.ToothUnit);
            StartCoroutine(nameof(SpinRoutine));
        }

        public void SetSpinDirection(ChainEnums.ChainDirection chainDirection)
        {
            Data.RotationDirection = chainDirection == ChainEnums.ChainDirection.Clockwise ? 1 : -1;
        }

        private float _speed;
        private void SetSpeedByTeeth()
        {
            _speed = ChainMover.MachinerySpeed / Data.TeethCount;
            //print("cog speed: " + _speed);
        }

        IEnumerator SpinRoutine()
        {
            var direction = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                ? Vector3.up * Data.RotationDirection
                : -Vector3.forward * Data.RotationDirection;

            while (true)
            {
                transform.Rotate(direction, _speed); //Todo: enum yapılabilir, yukarı aşağı sağ sol
                // transform.rotation =
                //     Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(direction), Data.Speed);
                yield return null;
            }
        }

        private void OnDisable()
        {
            ChainEvents.OnMotionStateSet -= StartMotion;
        }
    }
}