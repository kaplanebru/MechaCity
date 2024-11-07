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
        public BPSlotHolder bpSlotHolder;

        private PlayerPersona PlayerPersona; // = new();
        private BpTrackerList bpTrackerList = new();
        private BaseBlueprint currentBlueprint;

        public void Subscribe()
        {
            BpEventbus.UIEvents.OnInteraction += ChangeStateAndSetBp; //todo: Daha sonra, (datadaki değişkenleri ayırdıktan sonra) network obj olarak data gönderilir yaparız
            BpEventbus.OnSendingSelectionsForExecution += SendBpExecutionRequestByUser;
            BpEventbus.OnDirectBpExecution += TryExecuteBpBySystem;
            BpEventbus.LifespanEvents.OnRestore += RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker += RemoveExpiredBp;

            NetworkEventbus.ServerEvents.OnBpSelectionByServer += SetCurrentBpByServer;
            NetworkEventbus.ServerEvents.OnBpExecutionRequestByServer += TryExecuteBpBySystem;
            NetworkEventbus.ServerEvents.OnPlayerPersonaSet += PlayerPersona.SetPlayerPersona;

            TurnStatusEvents.OnTurnEnding += UpdateBpTrackers;
            bpTrackerList.Subscribe();
        }

        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            BpHolder.CreateBlueprints();
            PlayerPersona = new PlayerPersona(bpSlotHolder);
            Subscribe();
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

        private void SetCurrentBpByServer(BpType type, int level) //network call
        {
            currentBlueprint = BpHolder.AllBlueprints[type];
            BpEventbus.UIEvents.OnBpInstallBegin?.Invoke(type);

            currentBlueprint.Level = level;
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
            if (selectedItems == null) return; //TODO: ya des trackers sans items

            foreach (var item in selectedItems)
            {
                var tracker =
                    bpTrackerList.CreateTracker(currentBlueprint.Lifespan, item, currentBlueprint.Type); //TODO: LATER
                BpEventbus.LifespanEvents.OnTrackerRequest?.Invoke(tracker);
            }
        }

        private void RestoreFromBp(BpType type, uint selectedItem)
        {
            BpHolder.AllBlueprints[type]
                .TryRestoreAction(
                    selectedItem); //todo: bug. sadece 3 tane bp var. ama aynı bpnin birden fazla kullanımı olmalı, ve selected itemlerı farklı olmalı
        }

        public void Unsubscribe()
        {
            BpEventbus.OnSendingSelectionsForExecution -= SendBpExecutionRequestByUser;
            BpEventbus.OnDirectBpExecution -= TryExecuteBpBySystem;
            BpEventbus.UIEvents.OnInteraction -= ChangeStateAndSetBp;
            BpEventbus.LifespanEvents.OnRestore -= RestoreFromBp;
            BpEventbus.LifespanEvents.OnExpiredTracker -= RemoveExpiredBp;

            NetworkEventbus.ServerEvents.OnBpSelectionByServer -= SetCurrentBpByServer;
            NetworkEventbus.ServerEvents.OnBpExecutionRequestByServer -= TryExecuteBpBySystem;
            NetworkEventbus.ServerEvents.OnPlayerPersonaSet -= PlayerPersona.SetPlayerPersona;

            TurnStatusEvents.OnTurnEnding -= UpdateBpTrackers;
            bpTrackerList.Unsubscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}