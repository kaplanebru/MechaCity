using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Chain
{
    public class CogMover : MonoBehaviour, Mover
    {
        public float MachinerySpeed { get; set; }

        public int Id { get; set; }
       
        public CogData Data;

        private void OnEnable()
        {
            ChainEvents.OnMotionStateSet += StartMotion;
            Data = GetComponent<Cogwheel>().Data;
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
            if(Data.ContactType == ChainEnums.CogContactType.ChainRelated)
                ChainEvents.OnCogSpeedSet?.Invoke(Data.TeethCount, Data.ToothUnit);
            StartCoroutine(nameof(SpinRoutine));
        }

       
        
        public void SetSpinDirectionByCog(Cogwheel relatedCog)
        {
            Data.RotationDirection = relatedCog.Data.RotationDirection * -1;
        }
        
        private float _speed;

        private void SetSpeedByTeeth()
        {
            _speed = MachinerySpeed / Data.TeethCount;
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
        }

    }
}