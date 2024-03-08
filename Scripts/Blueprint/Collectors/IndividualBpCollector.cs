using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace Blueprint
{
  
    public class IndividualBpCollector: MonoBehaviour
    {
        public int towerId; //Todo: tower id ile aynı. Tower cs scriptlerinden ayrı çalışsın diye monobehaviour yaptım
        public List<BpLifeTracker> Collector = new();

        private void OnEnable()
        {
            Subscribe();
        }
        public void Subscribe()
        {
            BpEventbus.ManagementEvents.OnBpExpired += RemoveFromCollection;
        }

        public void SetId(int id)
        {
            towerId = id;
        }

        void AddToCollection(BpLifeTracker lifeTracker)
        {
            Collector.Add(lifeTracker);
            lifeTracker.SetId(towerId);
        }

        void RemoveFromCollection(BpType type)
        {
            var toRemove = Collector.FirstOrDefault(l=>l.type == type);
            Collector.Remove(toRemove);
        }
       

        public void Unsubscribe()
        {
            BpEventbus.ManagementEvents.OnBpExpired -= RemoveFromCollection;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}