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
            //ChainEvents.OnCogReady?.Invoke(this); // TODO: execution order sıkıntı
            ChainEvents.OnMotionStateSet += StartMotion;
        }

        private void Start()
        {
            ChainEvents.OnCogReady?.Invoke(this); // TODO: execution order sıkıntı

        }


        private void StartMotion(bool isMoving)
        {
            ChainEvents.OnCogSpeedSet?.Invoke(Data.TeethCount, Data.ToothUnit);

            //Setup();
            Data.IsMoving = isMoving;
            if (!Data.IsMoving) return;
            
            StartCoroutine(nameof(SpinRoutine));
        }

        public void SetSpinDirection(ChainEnums.ChainDirection chainDirection)
        {
            Data.RotationDirection = chainDirection == ChainEnums.ChainDirection.Clockwise ? 1 : -1;
        }
        
        IEnumerator SpinRoutine()
        {
            print("cog speed : " + Data.Speed);
            var direction = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                ? Vector3.up * Data.RotationDirection
                : -Vector3.forward * Data.RotationDirection;

            while (true)
            {
                transform.Rotate(direction, Data.Speed); //Todo: enum yapılabilir, yukarı aşağı sağ sol
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