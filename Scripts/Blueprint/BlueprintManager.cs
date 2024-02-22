using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Enums;
using Network;
using UnityEngine;

namespace Blueprint
{
    public class BlueprintManager : MonoBehaviour
    {
        public List<BpType> activeBlueprints = new();
        
        private BpHolder bpHolder = new BpHolder();
        //public Bp_StateIntruder BpStateIntruder = 
        public BPSlotHolder slotHolder;
        public BPDataHolder bpDataHolder;
        
        public void Subscribe()
        {
            NetworkEventbus.BlueprintEvents.OnBpSelected += StartBpExecution;
        }
        
        private void StartBpExecution(BpType type)
        {
            NetworkEventbus.BlueprintEvents.OnStateIntrusionAttempt?.Invoke();   //call intruder event for both players
            
            BaseBlueprint currentBlueprint = bpHolder.AllBlueprints[type];
            //show ui if bp has ui: ui paneli bütün clicklerin önüne geçsin
            
          
            
            currentBlueprint.TryTakeAction();
            NetworkEventbus.BlueprintEvents.OnStateIntrusionEnd?.Invoke();
            
        }

        void Start()
        {
           Initialize();
        }

        public void Initialize()
        {
            Subscribe();
            bpHolder.Initialize();
            GetActiveBlueprints();
            
            slotHolder.Setup(activeBlueprints);
        }
        
        public void GetActiveBlueprints() 
        {
            for (int i = 0; i < 3; i++) //TODO: Temp
            {
                activeBlueprints.Add(bpHolder.AllBlueprints.Keys.ElementAt(i));
            }
         
        }
        
        
        public void Unsubscribe()
        {
            NetworkEventbus.BlueprintEvents.OnBpSelected -= StartBpExecution;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }

}
