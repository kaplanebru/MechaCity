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
        private List<BpType> _activeBlueprints = new();
        private void OnEnable()
        {
            slots = GetComponentsInChildren<BPSlot>();
        }

        public void Setup(List<BpType> activeBlueprints) //LEVELA GÖRE VE PERSONAYA GÖRE
        {
            _activeBlueprints = activeBlueprints;

            for (var i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                slot.SetType(_activeBlueprints[i]);
                slot.Setup(bpDataHolder.TypeDataPair[slot.currentBpType]);
            }
        }
    }
}