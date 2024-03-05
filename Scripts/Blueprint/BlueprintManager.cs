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
        }

        private void ExecuteBp(object[] obj)
        {
            //instrucion uygulansın! currentBp.ready olana kadar.

            //waitwhile ready
            //ortadaki bp'ye basılınca take action with parameters: currentbpnin içindeki datalar dolar params obj datası. Ayrıca parametre olarak almaya gerek yok!

            currentBlueprint.TryTakeAction();
            //NetworkEventbus.BlueprintEvents.OnStateIntrusionEnd?.Invoke();
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
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}