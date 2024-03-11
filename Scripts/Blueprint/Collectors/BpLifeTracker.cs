using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;


namespace Blueprint
{
   [Serializable]
    public class BpLifeTracker //sadece enum tutabilir, sonra restore deriz
    {
        private BpType Type;
        private int Lifespan;

        public BpLifeTracker(BpType type, int lifespan = 1)
        {
            Type = type;
            Lifespan = lifespan;
        }

        
        public void ReduceLife()
        {
            Debug.Log("lifespan: "+Lifespan);
            Lifespan--;
            if (Lifespan < 0)
            {
                Debug.Log("expired");
                BpEventbus.LifespanEvents.OnRestore?.Invoke(Type);
                BpEventbus.LifespanEvents.OnBpExpired?.Invoke(this);
            }
        }
    }
}

