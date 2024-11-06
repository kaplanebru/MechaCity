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
            slots = GetComponentsInChildren<BPSlot>(true);
        }

        public void Setup(List<BpType> activeBlueprints) //LEVELA GÖRE VE PERSONAYA GÖRE
        {
            _activeBlueprints = activeBlueprints;
            DisableAll();

            for (var i = 0; i < activeBlueprints.Count; i++)
            {
                var slot = slots[i];
                slot.gameObject.SetActive(true);
                slot.SetType(_activeBlueprints[i]);
                slot.Setup(bpDataHolder.TypeDataPair[slot.currentBpType]);
            }
        }

        void DisableAll()
        {
            foreach (var slot in slots)
            {
                slot.gameObject.SetActive(false);
            }
        }
    }
}