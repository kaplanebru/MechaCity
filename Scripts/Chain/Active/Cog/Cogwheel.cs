using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [Serializable]
    public class CogData
    {
        public float Radius = 3;
        public float RadiusOffset = .5f;
        public float Speed = 10; //Todo: according to radius yap
        public Vector3 RotationDirection = -Vector3.up;
        
    }
    
    public class Cogwheel : MonoBehaviour
    {
        public CogData Data;

        private void OnEnable()
        {
            ChainEvents.OnStartAndMove += MoveCog;
        }

        private void MoveCog(bool isMoving)
        {
            Setup();
            if(isMoving)
                StartCoroutine(nameof(SpinRoutine));
        }
        

        void Setup()
        {
            var radius = Data.Radius + Data.RadiusOffset;
            var scale = transform.localScale;
            scale.x = radius;
            scale.z = radius;
            transform.localScale = scale; //Todo: offseti oran yap radius arttıkça oran büyüsün
        }
        IEnumerator SpinRoutine()
        {
            while (true)
            {
                transform.Rotate(Data.RotationDirection, Data.Speed);
                yield return null;
            }
        }

        private void OnDisable()
        {
            ChainEvents.OnStartAndMove -= MoveCog;
        }
    }

}
