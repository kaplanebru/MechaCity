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
        public int UniqId;

        private void OnEnable()
        {
            Subscribe();
        }
        public void Subscribe()
        {
            BpEventbus.LifespanEvents.OnBpAdded += AddToCollection;
        }

        public void SetId(int id)
        {
            UniqId = id;
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
        

        public void AddToCollection(BpType type)
        { print("add to coll");
            BpLifeTracker lifeTracker = new BpLifeTracker(UniqId, type);
            Collector.Add(lifeTracker);
        }

        public void RemoveFromCollection(BpLifeTracker lifeTracker)
        {
            Collector.Remove(lifeTracker);
        }
        
        public void Unsubscribe()
        {
            BpEventbus.LifespanEvents.OnBpAdded -= AddToCollection;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}