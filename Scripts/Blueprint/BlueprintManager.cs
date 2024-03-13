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
        private BaseBlueprint currentBlueprint;

        private BpHolder bpHolder = new BpHolder();

        //public Bp_StateIntruder BpStateIntruder = 
        public BPSlotHolder slotHolder;
        public BPDataHolder bpDataHolder;
        public BpTrackerList bpTrackerList = new ();

        
        private void Update() //TODO: TEST
        {
            if (Input.GetMouseButtonDown(1))
            {
                bpTrackerList.ReduceValueForAll();
            }
        }
        public void Subscribe()
        {
            NetworkEventbus.RequestEvents.OnBpSelectionByServer += SetCurrentBpForAll;
            NetworkEventbus.RequestEvents.OnBpExecutionBySystem += ExecuteBp;
            BpEventbus.LifespanEvents.OnRestore += RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker += RemoveExpiredBp;
            bpTrackerList.Subscribe();
        }
        
        private void RemoveExpiredBp(ITrackable lifeTracker)
        {
            bpTrackerList.RemoveFromTrackList(lifeTracker);
        }

        private void ExecuteBp(int[] selectedItems)
        {
            currentBlueprint.TryTakeAction(selectedItems);

            foreach (var item in selectedItems)
            {
                var tracker = bpTrackerList.CreateTracker(currentBlueprint.Lifespan, item, currentBlueprint.Type);
                BpEventbus.LifespanEvents.OnTrackerRequest?.Invoke(tracker);
            }
        }

        private void RestoreFromBp(BpType type, int selectedItem)
        {
           bpHolder.AllBlueprints[type].TryRestoreAction(selectedItem); //todo: bug. sadece 3 tane bp var. ama aynı bpnin birden fazla kullanımı olmalı, ve selected itemlerı farklı olmalı
        }

        private void SetCurrentBpForAll(BpType type) //network function
        {
            currentBlueprint = bpHolder.AllBlueprints[type]; //execution için 2 tarafta da bunun set edilmesi gerek
            BpEventbus.UIEvents.OnBpInstallBegin?.Invoke(type);
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
            NetworkEventbus.RequestEvents.OnBpSelectionByServer -= SetCurrentBpForAll;
            NetworkEventbus.RequestEvents.OnBpExecutionBySystem -= ExecuteBp;
            BpEventbus.LifespanEvents.OnRestore -= RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker -= RemoveExpiredBp;
            bpTrackerList.Unsubscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}