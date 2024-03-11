using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace Blueprint
{
  
    public class BpTrackerCollector: MonoBehaviour 
        //MB olmak zorunda değil. Previous state için factory patterne bakılabilir - freeze - unfreeze
        //bütün hepsi dinliyor, sıkıntı. tek bir yerden kule ve event birlikte dinlenebilir
    {
        public List<BpLifeTracker> Collector = new();

        private void OnEnable()
        {
            Subscribe();
        }
        public void Subscribe()
        {
            BpEventbus.LifespanEvents.OnBpAdded += AddToCollection;
            BpEventbus.LifespanEvents.OnBpExpired += RemoveFromCollection;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                for (var i = 0; i < Collector.Count; i++)
                {
                    var l = Collector[i];
                    l.ReduceLife();
                }
            }
        }
        

        void AddToCollection(BpType type)
        { print("add to coll");
            BpLifeTracker lifeTracker = new BpLifeTracker(type);
            Collector.Add(lifeTracker);
        }

        void RemoveFromCollection(BpLifeTracker lifeTracker)
        {
            Collector.Remove(lifeTracker);
        }
        
        public void Unsubscribe()
        {
            BpEventbus.LifespanEvents.OnBpAdded -= AddToCollection;
            BpEventbus.LifespanEvents.OnBpExpired -= RemoveFromCollection;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}