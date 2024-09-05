using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums.Selections;
using UnityEngine;

public class SelectionTable : MonoBehaviour
{
    public SelectionSlot[] slots;
    public GameObject content;
    
    private Selector _currentSelector;
    private List<SelectionSlot> emptySlots = new();
    private List<SelectionSlot> activeSlots = new();
    private int _slotAmount;

    private void OnEnable()
    {
        content.SetActive(false);
        
        SelectionEvents.OnSelectionReady += StartTable;
        SelectionEvents.OnSelectionTerminated += CloseTable;

        SelectionEvents.OnSelection += AddToTable;
        SelectionEvents.OnDeselect += RemoveFromTable;
        SelectionEvents.OnDeselectAll += ResetTable;
    }

    private void ResetTable()
    {
        foreach (var slot in activeSlots)
        {
            slot.ResetSlot();
        }

        emptySlots = activeSlots.ToList();
    }

    private void AddToTable(string towerName, int id)
    {
        emptySlots.First().Fill(towerName, id);
        emptySlots.RemoveAt(0);
        
        //dolmuşsa resetlemek lazım
    }
    
    private void RemoveFromTable(int id)
    {
        var slot = slots.First(s => s.towerId == id);
        slot.ResetSlot();
        
        emptySlots.Add(slot);
        Reorder();
    }

    private void Reorder()
    {
        emptySlots = emptySlots.OrderBy(s => s.Index).ToList();
    }

    public void ResetSelections()
    {
        _currentSelector.DeselectAll();
    }
    
    
    //_______________SETTER____________________________________
    
    private void Start()
    {
        SetIndexesForOnce();
    }

    public void StartTable(Selector selector)
    {
        content.SetActive(true);
        _currentSelector = selector;
        SetSlots();
    }
    
    private void CloseTable()
    {
        content.SetActive(false);
    }

    
    public void SetSlots()
    {
        activeSlots.Clear();
        emptySlots.Clear();
        
        DisableAll();
        SetSlotAmountAnColors();

        for (int i = 0; i < _slotAmount; i++)
        {
            var slot = slots[i];
            
            slot.ResetSlot();
            slot.gameObject.SetActive(true);
            emptySlots.Add(slot);
            activeSlots.Add(slot);
        }
    }
    void SetSlotAmountAnColors()
    {
        _slotAmount = 0;
        foreach (var group in _currentSelector.Data.Groups)
        {
            for (int i = 0; i < group.MaxTowers; i++)
            {
                slots[_slotAmount + i].SetTeamColor(_currentSelector.GetSelectionTeam(group.Index));
            }
            _slotAmount += group.MaxTowers;
           
        }
    }

    private void SetIndexesForOnce()
    {
        for (var i = 0; i < slots.Length; i++)
        {
            slots[i].SetIndex(i);
        }
    }
    
    private void DisableAll()
    {
        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        SelectionEvents.OnSelectionReady -= StartTable;
        SelectionEvents.OnSelectionTerminated -= CloseTable;

        SelectionEvents.OnSelection -= AddToTable;
        SelectionEvents.OnDeselect -= RemoveFromTable;
        SelectionEvents.OnDeselectAll -= ResetTable;

    }
}