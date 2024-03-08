using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Blueprint
{
    [Serializable]
    public class BpLifeTracker
    {
        public int towerId;
        public BpType type;
        public int lifespan;
        public List<int> relatedTowers;

        public void SetId(int id)
        {
            towerId = id;
        }
        
        public void ReduceLife()
        {
            lifespan--;
            if (lifespan == 0)
            {
                BpEventbus.ManagementEvents.OnBpExpired?.Invoke(type);
            }
        }
    }
}

