using System;
using System.Collections;
using System.Collections.Generic;
using Enums.Selections;
using UnityEngine;

public class SelectionTable : MonoBehaviour
{
    private Selector _currentSelector;
    public SelectionSlot[] slots;
    private int _slotAmount;

    private void OnEnable()
    {
        SelectionEvents.OnBlockerSet += GetSelector;
    }

    public void GetSelector(Selector selector)
    {
        _currentSelector = selector;
        SetSlots();
    }
    
    public void SetSlots()
    {
        DisableAll();
        SetSlotAmountAnColors();

        for (int i = 0; i < _slotAmount; i++)
        {
            var slot = slots[i];
            
            slot.ResetName();
            slot.gameObject.SetActive(true);
        }
    }
    void SetSlotAmountAnColors()
    {
        _slotAmount = 0;
        foreach (var group in _currentSelector.Data.Groups)
        {
            for (int i = 0; i < group.MaxTowers; i++)
            {
                slots[_slotAmount + i].SetTeamColor(_currentSelector.GetSelectionTeam());
            }
            _slotAmount += group.MaxTowers;
           
        }
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
        SelectionEvents.OnBlockerSet -= GetSelector;
    }

    //not: eşleştirmeyle uğraşma zaten blocklular
}