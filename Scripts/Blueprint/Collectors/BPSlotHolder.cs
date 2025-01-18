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
        [SerializeField]private CardSlot[] slots;
        private CardSlotFront frontSlot;
        private List<BpType> _activeBlueprintTypes = new();
        private void OnEnable()
        {
            slots = GetComponentsInChildren<CardSlot>(true);
            frontSlot = slots[0] as CardSlotFront;
            
            BpEventbus.ActionEvents.OnBpActionCompleted += ActivateSlot;
            BpEventbus.SelectionEvents.OnCardSelectionApplied += SelectBpSlot;
            BpEventbus.CardEvents.OnCardSelection += ShiftToFrontSlot;

        }
        
        public void Setup(List<BpType> activeBlueprintTypes) //LEVELA GÖRE VE PERSONAYA GÖRE
        {
            _activeBlueprintTypes = activeBlueprintTypes;
            DisableAll();
            
            for (var i = 0; i < activeBlueprintTypes.Count; i++)
            {
                SetSlot(slots[i], i);
            }
        }
        
        private void SetSlot(CardSlot slot, int typeIndex)
        {
            slot.gameObject.SetActive(true);
            slot.Setup(bpDataHolder.TypeDataPair[_activeBlueprintTypes[typeIndex]]);
        }
        
        private void ShiftToFrontSlot(BpType selectedType)
        {
            var oldFrontType = frontSlot.Data.Type;
            if(oldFrontType == selectedType) return;
            
            var oldFrontData = bpDataHolder.TypeDataPair[oldFrontType];
            var dataToShift = bpDataHolder.TypeDataPair[selectedType];
            var slotToShift = slots.FirstOrDefault(s => s.currentBpType == selectedType);
            
            frontSlot.Setup(dataToShift);
            slotToShift.Setup(oldFrontData);
        }
        

        private void SelectBpSlot(BpType type)
        {
            BpEventbus.SelectionEvents.OnBpSlotSelected?.Invoke(type, bpDataHolder.TypeDataPair[type].Level);
            var slot = slots.FirstOrDefault(s => s.Data.Type == type);
            slot.Deactivate(); //TODO: clientlara bu kart seçildi diye mesaj gitsin, ya da herkese işte.
        }

        private void ActivateSlot(BpType type)
        {
            var slot = slots.FirstOrDefault(b => b.Data.Type == type);
            slot.Activate();
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
            BpEventbus.SelectionEvents.OnCardSelectionApplied -= SelectBpSlot;
            BpEventbus.CardEvents.OnCardSelection -= ShiftToFrontSlot;
        }
    }
}