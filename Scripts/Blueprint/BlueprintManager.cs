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

        public void Subscribe()
        {
            NetworkEventbus.RequestEvents.OnBpSelectionByServer += GetCurrentBp;
            BpEventbus.TriggerEvents.OnBpCompletedByButton += BpRequest;
            NetworkEventbus.RequestEvents.OnBpExecutionBySystem += ExecuteBp;
        }

        public void BpRequest(List<int> selectedTowers)
        {
            NetworkEventbus.TriggerEvents.OnBpExecutionRequestByUser?.Invoke(currentBlueprint.Type, selectedTowers.ToArray());
        }

        private void ExecuteBp(BpType type, int[] selectedElements)
        {
            //ortaya tıklanabilir!
            currentBlueprint.Type = type;
            currentBlueprint.SelectedElements = selectedElements;

            currentBlueprint.TryTakeAction(); //selected elements ekle
            NetworkEventbus.BlueprintEvents.OnStateIntrusionEnd?.Invoke(); //TODO: Dont! not state değişim yollarsak 2 kez gidecek! yA DA BUTONA TIKLANINCA STATE DEĞİŞİR ZATEN
            //BELKİ DE BP NETWORK VARİBALE GEREKİR

        }

        private void GetCurrentBp(BpType type)
        {
            currentBlueprint = bpHolder.AllBlueprints[type]; //execution için 2 tarafta da bunun set edilmesi gerek
            NetworkEventbus.BlueprintEvents.OnBpInstallBegin?.Invoke(type);
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
            NetworkEventbus.RequestEvents.OnBpSelectionByServer -= GetCurrentBp;
            BpEventbus.TriggerEvents.OnBpCompletedByButton -= BpRequest;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}