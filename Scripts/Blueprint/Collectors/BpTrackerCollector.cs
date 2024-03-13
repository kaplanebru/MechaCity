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

      

        public void AddToCollection(BpType type)
        { 
            print("add to coll");
            // BpLifeTracker lifeTracker = new BpLifeTracker(type, 1);
            // Collector.Add(lifeTracker);
        }

        public void RemoveFromCollection(BpLifeTracker lifeTracker)
        {
            Collector.Remove(lifeTracker);
        }
        
       

    
    }
}