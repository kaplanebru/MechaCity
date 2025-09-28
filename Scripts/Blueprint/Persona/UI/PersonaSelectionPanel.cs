using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Network;
using PlayerNetwork;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;


public class PersonaSelectionPanel : MonoBehaviour
{
    [SerializeField] private PersonaSlotData[] slotsData;
    [SerializeField] private GameObject content;
    [SerializeField] private float delay = 1;
    private PersonaSlot[] slots;
    private PersonaType selectedType;

    private void OnEnable()
    {
        NetworkEventbus.ServerEvents.OnPlayerSpawned += EnableContent;
        BpEventbus.PersonaEvents.OnPersonaSlotClicked += PersonaSlotClicked;
    }

    private void EnableContent(Player arg1, ulong arg2)
    {
        content.SetActive(true);
        SetSlots();
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
        selectedType = type;
        DisableOthers();
        NetworkEventbus.UserEvents.OnPersonaSelectedByUser?.Invoke(selectedType);
        
        Invoke(nameof(DisableThis), delay);
    }

    void DisableThis()
    {
        gameObject.SetActive(false);
    }
    
    private void DisableOthers()
    {
        foreach (var slot in slots)
        {
            if(slot.Type == selectedType) continue;
            slot.Cancel();
        }
    }

    private void OnDisable()
    {
        BpEventbus.PersonaEvents.OnPersonaSlotClicked -= PersonaSlotClicked;
        NetworkEventbus.ServerEvents.OnPlayerSpawned -= EnableContent;
    }
}

[Serializable]
public class PersonaSlotData
{
    public PersonaType Type;
    public Sprite Sprite;
}