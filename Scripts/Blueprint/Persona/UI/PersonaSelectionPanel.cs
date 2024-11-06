using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Network;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;


public class PersonaSelectionPanel : MonoBehaviour
{
    public PersonaSlotData[] slotsData;
    private PersonaSlot[] slots;

    private void OnEnable()
    {
        SetSlots();
        BpEventbus.PersonaEvents.OnPersonaSlotClicked += PersonaSlotClicked;
    }

    private void SetSlots()
    {
        slots = GetComponentsInChildren<PersonaSlot>();
        for (var i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            slot.Setup(slotsData[i]);
        }
    }

    public void PersonaSlotClicked(PersonaType type)
    {
        DisableAll();
        NetworkEventbus.UserEvents.OnPersonaSelectedByUser?.Invoke(type);
    }
    

    private void DisableAll()
    {
        foreach (var slot in slots)
        {
            slot.button.interactable = false;
        }
    }

    private void OnDisable()
    {
        BpEventbus.PersonaEvents.OnPersonaSlotClicked -= PersonaSlotClicked;
    }
}

[Serializable]
public class PersonaSlotData
{
    public PersonaType Type;
    public Sprite Sprite;
}