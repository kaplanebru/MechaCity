using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Enums;
using JetBrains.Annotations;
using Network;
using UnityEngine;

namespace Blueprint
{
    public class BlueprintManager : MonoBehaviour
    {
        public List<BpType> activeBlueprints = new();
        private BaseBlueprint currentBlueprint;

        private BpHolder bpHolder = new BpHolder();

        public BPSlotHolder slotHolder;
        public BPDataHolder bpDataHolder;
        public BpTrackerList bpTrackerList = new ();

        
        public void Subscribe()
        {
            TurnStatusEvents.OnTurnEnding += UpdateBpTrackers;

            BpEventbus.UIEvents.OnInteraction += ChangeStateAndSetBp; //todo: Daha sonra, (datadaki değişkenleri ayırdıktan sonra) network obj olarak data gönderilir yaparız
            
            NetworkEventbus.RequestEvents.OnBpSelectionByServer += SetCurrentBpByServer;
            
            BpEventbus.OnSendingSelectionsForExecution += TryExecuteBp;
            
            BpEventbus.LifespanEvents.OnRestore += RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker += RemoveExpiredBp;
            bpTrackerList.Subscribe();
        }

        private void ChangeStateAndSetBp(BpType type, int level)
        {
            StartCoroutine(BpSelectionDelay(type, level));
        }

        IEnumerator BpSelectionDelay(BpType type, int level) //On Interaction : calls network
        {
            BpEventbus.StateEvents.StateChangeRequestToIntruder?.Invoke(TurnStateType.Intruder);
            yield return new WaitForSeconds(.2f);
            NetworkEventbus.TriggerEvents.OnSetCurrentBpRequestByUser?.Invoke(type, level);
        }
        
        private void SetCurrentBpByServer(BpType type,int level) //network call
        {
            currentBlueprint = bpHolder.AllBlueprints[type];
            BpEventbus.UIEvents.OnBpInstallBegin?.Invoke(type);

            currentBlueprint.Level = level;
            
            print("current bp: " + currentBlueprint);

            BpEventbus.SelectionEvents.OnCurrentBpSet?.Invoke(currentBlueprint.SelectionType);
        }

       
        private void UpdateBpTrackers()
        {
            bpTrackerList.ReduceValueForAll();
        }
        private void RemoveExpiredBp(ITrackable lifeTracker)
        {
            bpTrackerList.RemoveFromTrackList(lifeTracker);
        }

        private void TryExecuteBp([CanBeNull] uint[] selectedItems)
        {
            //NETWORK
            if (currentBlueprint.TryTakeAction(selectedItems))
            {
                SetTracker(selectedItems);
                //BpEventbus.StateEvents.OnStateChangeRequestFromIntruder?.Invoke();
            }
        }

        void SetTracker([CanBeNull] uint[] selectedItems)
        {
            if(selectedItems == null) return; //TODO: ya des trackers sans items
            
            foreach (var item in selectedItems)
            {
                var tracker = bpTrackerList.CreateTracker(currentBlueprint.Lifespan, item, currentBlueprint.Type);  //TODO: LATER
                BpEventbus.LifespanEvents.OnTrackerRequest?.Invoke(tracker);
            }
        }

        private void RestoreFromBp(BpType type, uint selectedItem)
        {
           bpHolder.AllBlueprints[type].TryRestoreAction(selectedItem); //todo: bug. sadece 3 tane bp var. ama aynı bpnin birden fazla kullanımı olmalı, ve selected itemlerı farklı olmalı
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
            for (int i = 0; i <bpHolder.AllBlueprints.Count; i++) //TODO: Temp
            {
                activeBlueprints.Add(bpHolder.AllBlueprints.Keys.ElementAt(i));
            }
        }


        public void Unsubscribe()
        {
            BpEventbus.OnSendingSelectionsForExecution -= TryExecuteBp;

            BpEventbus.UIEvents.OnInteraction -= ChangeStateAndSetBp;
            TurnStatusEvents.OnTurnEnding -= UpdateBpTrackers;
            NetworkEventbus.RequestEvents.OnBpSelectionByServer -= SetCurrentBpByServer;
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