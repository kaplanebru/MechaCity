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
        public int Lifespan { get; set; }
        public uint RelatedActor { get; set; }
        public BpLifeTracker(int lifespan, uint relatedActor, BpType type)
        {
            Lifespan = lifespan;
            RelatedActor = relatedActor;
            Type = type;
        }

        

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
                BpEventbus.LifespanEvents.OnRestore?.Invoke(Type, RelatedActor);//bpmanagera gidiyor sorun yok
                BpEventbus.LifespanEvents.OnExpiredTracker?.Invoke(this);
            }
            Debug.Log("lifespan: "+Lifespan);
        }
    }
}

