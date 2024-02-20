using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using Enums;
using UnityEngine;

public class BPSlotHolder : MonoBehaviour
{
    public BPHolder bpHolder;
    public BPSlot[] slots;
   

    private void Start() //TODO: Initialize
    {
        slots = GetComponentsInChildren<BPSlot>();
        Setup();
    }

    void Setup()
    {
        for (var i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            slot.Setup(bpHolder.BPData[i]);
        }
    }
}
