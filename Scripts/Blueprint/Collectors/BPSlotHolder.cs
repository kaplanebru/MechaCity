using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class BPSlotHolder : MonoBehaviour
    {
        public BPDataHolder bpDataHolder;
        public CardSlot[] slots;
        private List<BpType> _activeBlueprints = new();
        private void OnEnable()
        {
            slots = GetComponentsInChildren<CardSlot>(true);
            BpEventbus.ActionEvents.OnBpActionCompleted += ActivateSlot;
        }

        private void ActivateSlot(BpType type)
        {
            var slot = slots.FirstOrDefault(b => b.Data.Type == type);
            slot.cardInteraction.Activate();
        }

        public void Setup(List<BpType> activeBlueprints) //LEVELA GÖRE VE PERSONAYA GÖRE
        {
            _activeBlueprints = activeBlueprints;
            DisableAll();
            
            for (var i = 0; i < activeBlueprints.Count; i++)
            {
                if (i == 0)
                    SetFrontSlot();
                else
                    SetSlot(slots[i], i);
            }
        }


        private void SetSlot(CardSlot slot, int index)
        {
            slot.gameObject.SetActive(true);
            slot.SetType(_activeBlueprints[index]);
            slot.Setup(bpDataHolder.TypeDataPair[slot.currentBpType]);
        }
        
        private void SetFrontSlot()
        {
            CardSlotFront front = slots[0] as CardSlotFront;
            SetSlot(front, 0);
            // front.gameObject.SetActive(true);
            // front.SetType(_activeBlueprints[0]);
            // front.Setup(bpDataHolder.TypeDataPair[front.currentBpType]);
            front.SetReliefModel();
        }
        

        void DisableAll()
        {
            foreach (var slot in slots)
            {
                slot.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            BpEventbus.ActionEvents.OnBpActionCompleted -= ActivateSlot;
        }
    }
}