using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Health
{
    public class HealthIcon : MonoBehaviour
    {
        public float range = 5;
        public float speed = .3f;

        private Vector2 _eulers;

        private void OnEnable()
        {
            // _eulers[0] = GetRandomAngle();
            // _eulers[1] = GetRandomAngle();
        }

        private void Update()
        {
            Move();
        }

    
        void Move()
        {
            transform.Rotate(new Vector3(_eulers[0], 0, 0).normalized * speed * Time.deltaTime);
        
        }

        public void SetRotation(Vector2 eulers)
        {
            _eulers = eulers;
       
        }
    
        float GetRandomAngle()
        {
            float random;
            do
            {
                random = Random.Range(-range, range);
            } while (range == 0);
    
            return random;
        }
    }
}
