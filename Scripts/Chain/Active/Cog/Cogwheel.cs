using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Chain
{
    public class Cogwheel : MonoBehaviour, CogComponent
    {
        public int Id { get; set; }
        public bool drawGizmos = false;


        public CogData Data;
        public Transform cogObject;
        public Hole[] holes;
        public Transform teeth;

        private void OnEnable()
        {
            ChainEvents.OnMotionStateSet += StartMotion;
            ChainEvents.OnNewCogData += AddData;
        }

        private void Start()
        {
            StartMotion(Data.IsMoving); //after edit
        }

        public void AddData(CogData data)
        {
            Data = data;
        }


        private void StartMotion(bool isMoving)
        {
            Data.IsMoving = isMoving;
            if (!Data.IsMoving) return;

            SetSpeedByTeeth();
            if(Data.ContactType == ChainEnums.CogContactType.ChainRelated)
                ChainEvents.OnCogSpeedSet?.Invoke(Data.TeethCount, Data.ToothUnit);
            StartCoroutine(nameof(SpinRoutine));
        }

        public void SetSpinDirectionByChain(ChainEnums.ChainDirection chainDirection)
        {
            Data.RotationDirection = chainDirection == ChainEnums.ChainDirection.Clockwise ? 1 : -1;
        }
        
        public void SetSpinDirectionByCog(Cogwheel relatedCog)
        {
            Data.RotationDirection = relatedCog.Data.RotationDirection * -1;
        }
        
        private float _speed;

        private void SetSpeedByTeeth()
        {
            _speed = ChainMover.MachinerySpeed / Data.TeethCount;
            //print("cog speed: " + _speed);
        }

        IEnumerator SpinRoutine()
        {
            var direction = Vector3.up * Data.RotationDirection;

            while (true)
            {
                transform.Rotate(direction, _speed); 
                // transform.rotation =
                //     Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(direction), Data.Speed);
                yield return null;
            }
        }

        private void OnDisable()
        {
            ChainEvents.OnMotionStateSet -= StartMotion;
            ChainEvents.OnNewCogData -= AddData;
        }

    }
}