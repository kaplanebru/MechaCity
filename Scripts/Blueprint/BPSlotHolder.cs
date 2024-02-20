using System.Collections;
using System.Collections.Generic;
using DataModels;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class BPSlotHolder : MonoBehaviour
    {
        public BPDataHolder bpDataHolder;
        public BPSlot[] slots;

        private BpHolder bpHolder = new BpHolder();


        private void OnEnable()
        {
            bpHolder.Subscribe();
        }

        private void Start() //TODO: Initialize
        {
            bpHolder.Initialize();
            slots = GetComponentsInChildren<BPSlot>();
            Setup();
           
        }

        void Setup()
        {
            bpHolder.GetActiveBlueprints();
            for (var i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                slot.SetType(bpHolder.activeBlueprints[i]);
                slot.Setup(bpDataHolder.TypeDataPair[slot.currentBpType]);
            }
        }

        private void OnDisable()
        {
            bpHolder.Unsubscribe();
        }
    }
}