using System;
using System.Collections;
using UnityEngine;

namespace Chain
{
    public class CogMover : MonoBehaviour, CogComponent, Mover
    {
        public float MachinerySpeed { get; set; }
        public int MachineryId { get; set; }
        public ChainEnums.ChainDirection MachineryDirection { get; set; }

        public int CogId { get; set; }
       
        public GearData Data;
        
        private float _speed;

        public void StartMotion()
        {
            if(Data.IsMoving)
                ProcessMotion();
        }

        public void StopMotion()
        {
            StopCoroutine(nameof(SpinRoutine));
        }

        public void MachinerySetup(float machinerySpeed, int machineryId, IMachinePartData machinePartData, ChainEnums.ChainDirection direction)
        {
            MachinerySpeed = machinerySpeed;
            MachineryId = machineryId;
            Data = machinePartData as GearData;
            MachineryDirection = direction;
        }

        void ProcessMotion()
        {
            if(Data.WithoutTeeth) return;
            SetSpeedByTeeth();
            SendLinearSpeed();
            StartCoroutine(nameof(SpinRoutine));
        }

        private void SendLinearSpeed()
        {
            if (Data.ContactType == ChainEnums.CogContactType.ChainRelated)
            {
                ChainEvents.OnCogSpeedSet?.Invoke(_speed * Data.Radius, MachineryId);
            }
        }

        public int ConvertedChainDirection()
        {
            return MachineryDirection switch
            {
                ChainEnums.ChainDirection.Clockwise => 1,
                ChainEnums.ChainDirection.ReverseClock => -1,
                ChainEnums.ChainDirection.None => 1,
                _ => throw new ArgumentOutOfRangeException()
            };
            //return MachineryDirection == ChainEnums.ChainDirection.Clockwise ? 1 : -1;
        }
        
        void SetSpinDirection()
        {
            if (Data.ContactType == ChainEnums.CogContactType.ChainRelated)
                Data.RotationDirection = ConvertedChainDirection();
            
            else if (Data.ContactType == ChainEnums.CogContactType.GearRelated)
            {
                if (Data.relatedGearData == null)
                    Debug.LogWarning("Related Cog of " + Data.name + " is empty");
                
                else if (Data.relatedGearData.ContactType == ChainEnums.CogContactType.ChainRelated)
                    Data.RotationDirection = ConvertedChainDirection() * -1;
                
                else if (Data.relatedGearData.ContactType == ChainEnums.CogContactType.Indifferent)
                    Data.RotationDirection = Data.relatedGearData.RotationDirection * -1;

                else
                {
                    Debug.LogWarning("2 'CogRelated' cogs can't work, change one of the cog's contact type!");
                }
                
            }
        }
        
        
        private void SetSpeedByTeeth()
        {
            _speed = MachinerySpeed / Data.TeethCount;
        }

        IEnumerator SpinRoutine()
        {
            SetSpinDirection();
            var direction = Vector3.up * Data.RotationDirection;
            
            while (true)
            {
                transform.Rotate(direction, _speed); 
                // transform.rotation =
                //     Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(direction), Data.Speed);
                yield return null;
            }
        }
        

    }
}