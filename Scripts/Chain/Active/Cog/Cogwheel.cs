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
        public float RadiusOffset = .5f;
        public float Speed = 10; //Todo: according to radius yap
        public int RotationDirection = 1;
        
    }
    
    public class Cogwheel : MonoBehaviour
    {
        public CogData Data;

        private void OnEnable()
        {
            ChainEvents.OnStartAndMove += MoveCog;
        }

        void Setup()
        {
            var radius = Data.Radius; // + Data.RadiusOffset;
            var scale = transform.localScale;
            scale.x = radius * 2;
            scale.z = radius * 2;
            transform.localScale = scale; //Todo: offseti oran yap radius arttıkça oran büyüsün
        }

        public void SetRotationDirection(ChainDirection chainDirection)
        {
            Data.RotationDirection = chainDirection == ChainDirection.Clockwise ? 1 : -1;
        }
        
        private void MoveCog(bool isMoving)
        {
            Setup();
            if(isMoving)
                StartCoroutine(nameof(SpinRoutine));
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
            ChainEvents.OnStartAndMove -= MoveCog;
        }
    }

}
