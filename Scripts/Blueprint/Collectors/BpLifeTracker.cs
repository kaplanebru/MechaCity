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
        private int Id;
        private BpType Type;
        private int Lifespan;

        public BpLifeTracker(int id, BpType type, int lifespan = 1)
        {
            Id = id;
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
                BpEventbus.LifespanEvents.OnRestore?.Invoke(Type); //managera gidiyor sorun yok
                BpEventbus.LifespanEvents.OnBpExpired?.Invoke(this, Id); //yine bütün listelerden çıkar bug olur
            }
        }
    }
}

