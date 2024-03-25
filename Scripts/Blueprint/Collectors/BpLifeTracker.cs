using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;


namespace Blueprint
{
   [Serializable]
    public class BpLifeTracker: ITrackable //sadece enum tutabilir, sonra restore deriz
    {
        private BpType Type;
        public BpLifeTracker(int lifespan, int relatedTower, BpType type)
        {
            Lifespan = lifespan;
            RelatedTower = relatedTower;
            Type = type;
        }

        public int Lifespan { get; set; }
        public int RelatedTower { get; set; }

        private bool skipMainTurn = true;
        
        public void ReduceValue()
        {
            //todo: bp'in playerı mı rivali mi etkilediğine göre lifespan geri sayılır - player oriented-rival oriented-both
            if (skipMainTurn)
            {
                skipMainTurn = false;
            }
            else
            {
                skipMainTurn = true;
                Lifespan--;
            }

            if (Lifespan <= 0)
            {
                Debug.Log("expired");
                BpEventbus.LifespanEvents.OnRestore?.Invoke(Type, RelatedTower);//bpmanagera gidiyor sorun yok
                BpEventbus.LifespanEvents.OnExpiredTracker?.Invoke(this);
            }
            Debug.Log("lifespan: "+Lifespan);
        }
    }
}

