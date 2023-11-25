using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Chain
{
    public class CogMover : MonoBehaviour, CogComponent, Mover
    {
        public float MachinerySpeed { get; set; }
        public int MachineryId { get; set; }

        public int CogId { get; set; }
       
        public CogData Data;
        
        private float _speed;


        private void OnEnable()
        {
            //ChainEvents.OnMotionStateSet += SendSpeed;
        }

        public void MachinerySetup(float machinerySpeed, int machineryId, IMachinePartData machinePartData)
        {
            MachinerySpeed = machinerySpeed;
            MachineryId = machineryId;
            Data = machinePartData as CogData;
            if(Data.IsMoving)
                ProcessMotion();
        }

        void ProcessMotion()
        {
            SetSpeedByTeeth();
            SendTeethInfo();
            StartCoroutine(nameof(SpinRoutine));
        }

        private void SendTeethInfo()
        {
            if (Data.ContactType == ChainEnums.CogContactType.ChainRelated)
                ChainEvents.OnCogSpeedSet?.Invoke(_speed * Data.Radius, MachineryId); //(Data.TeethCount, Data.ToothUnit, MachineryId);
        }

        
        public void SetSpinDirectionByCog(Cogwheel relatedCog)
        {
            Data.RotationDirection = relatedCog.Data.RotationDirection * -1;
        }
        

        private void SetSpeedByTeeth()
        {
            _speed = MachinerySpeed / Data.TeethCount;
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
            //ChainEvents.OnMotionStateSet -= SendSpeed;
        }

    }
}