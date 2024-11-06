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
       
        public Persona playerPersona;
        public List<BpType> activeBlueprints = new();
        private BaseBlueprint currentBlueprint;
        
        public BPSlotHolder slotHolder;
        public BPDataHolder bpDataHolder;
        public BpTrackerList bpTrackerList = new ();

        
        public void Subscribe()
        {
            TurnStatusEvents.OnTurnEnding += UpdateBpTrackers;

            BpEventbus.UIEvents.OnInteraction += ChangeStateAndSetBp; //todo: Daha sonra, (datadaki değişkenleri ayırdıktan sonra) network obj olarak data gönderilir yaparız
            
            NetworkEventbus.ServerEvents.OnBpSelectionByServer += SetCurrentBpByServer;
            NetworkEventbus.ServerEvents.OnBpExecutionRequestByServer += TryExecuteBpBySystem;
            
            BpEventbus.OnSendingSelectionsForExecution += SendBpExecutionRequestByUser;
            BpEventbus.OnDirectBpExecution += TryExecuteBpBySystem;
            
            BpEventbus.LifespanEvents.OnRestore += RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker += RemoveExpiredBp;
            
            NetworkEventbus.ServerEvents.OnPlayerPersonaSet += SetPlayerPersona;
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
            NetworkEventbus.UserEvents.OnSetCurrentBpRequestByUser?.Invoke(type, level);
        }
        
        private void SetCurrentBpByServer(BpType type,int level) //network call
        {
            currentBlueprint = BpHolder.AllBlueprints[type];
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

        private void SendBpExecutionRequestByUser(uint[] selectedItems)
        {
            NetworkEventbus.UserEvents.OnBpExecutionRequestByUser?.Invoke(selectedItems);
        }
        
        private void TryExecuteBpBySystem([CanBeNull] uint[] selectedItems)
        {
            if (currentBlueprint.TryTakeAction(selectedItems))
            {
                SetTracker(selectedItems);
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
           BpHolder.AllBlueprints[type].TryRestoreAction(selectedItem); //todo: bug. sadece 3 tane bp var. ama aynı bpnin birden fazla kullanımı olmalı, ve selected itemlerı farklı olmalı
        }
        
        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            Subscribe();
            
            //BpHolder.CreateBlueprints();
            //GetActiveBlueprints();
            //slotHolder.Setup(activeBlueprints);
        }

        private OtherBpProvider _otherBpProvider;
        public void GetActiveBlueprints()
        {
          
            activeBlueprints.Clear();
            activeBlueprints.AddRange(playerPersona.Data.BpTypes);
            Debug.Log(playerPersona.Type);
            //activeBlueprints.AddRange(_otherBpProvider.GetBlueprints(playerPersona.Type, 1));
        }
        
        private void SetPlayerPersona(PersonaType type)
        {
            playerPersona = PersonaHolder.GetPersona(type);
            Debug.Log(playerPersona.Type);
           
            BpHolder.CreateBlueprints();
            GetActiveBlueprints();
            slotHolder.Setup(activeBlueprints);
        }


        public void Unsubscribe()
        {
            BpEventbus.OnSendingSelectionsForExecution -= SendBpExecutionRequestByUser;
            BpEventbus.OnDirectBpExecution -= TryExecuteBpBySystem;


            BpEventbus.UIEvents.OnInteraction -= ChangeStateAndSetBp;
            TurnStatusEvents.OnTurnEnding -= UpdateBpTrackers;
            NetworkEventbus.ServerEvents.OnBpSelectionByServer -= SetCurrentBpByServer;
            NetworkEventbus.ServerEvents.OnBpExecutionRequestByServer -= TryExecuteBpBySystem;
            BpEventbus.LifespanEvents.OnRestore -= RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker -= RemoveExpiredBp;
            NetworkEventbus.ServerEvents.OnPlayerPersonaSet -= SetPlayerPersona;

            bpTrackerList.Unsubscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}