using System.Collections;
using UnityEngine;

namespace ChainDemo
{
    public class PoleMotion : MonoBehaviour
    {
        public PoleData Data;
    
        float LimitUp { get; set; }
        Vector2 Limits { get; set; }
    
        private float Height { get; set; }
        int direction;
        private Vector3 startPos;
    
        void SetVariables()
        {
            do
            {
                direction = Random.Range(-1, 1);
            } while (direction == 0);
    
            Height = Data.height * direction;
            
            SetLimits();
        }
    
    
        void SetLimits()
        {
            LimitUp = Random.Range(Data.limit - Data.randomAmount, Data.limit + Data.randomAmount);
            Limits = new(startPos.y - Data.limitDown, startPos.y + LimitUp);
        }
    
        void Start()
        {
            startPos = transform.localPosition;
            SetVariables();
            StartCoroutine(nameof(MoveRoutine));
        }
    
        IEnumerator MoveRoutine()
        {
            while (true)
            {
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    transform.localPosition + Vector3.up * Height,
                    Data.speed);
    
                if (transform.localPosition.y > Limits.y || transform.localPosition.y < Limits.x)
                {
                    Height *= -1;
                    SetLimits();
                }
    
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
