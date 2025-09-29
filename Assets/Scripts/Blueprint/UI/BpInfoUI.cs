using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using Enums;
using Network;
using TMPro;
using UnityEngine;

public class BpInfoUI : MonoBehaviour
{
    public BPDataHolder bpDataHolder;
    public GameObject panelObject;
    public TextMeshProUGUI titleSlot;
    public TextMeshProUGUI instructionSlot;
    
    private BlueprintData _currentData;

    private void OnEnable()
    {
        panelObject.SetActive(false);
        BpEventbus.UIEvents.OnBpInstalled += ShowPanel;
    }

    private async void ShowPanel(BpType type)
    {
        _currentData = bpDataHolder.TypeDataPair[type];
        SetPanel();
        panelObject.SetActive(true);
        await DelayMaker.WaitForSeconds(3f);
        panelObject.SetActive(false);
    }

    void SetPanel()
    {
        titleSlot.text = _currentData.Title;
        instructionSlot.text = _currentData.Instruction;
    }


    private void OnDisable()
    {
        BpEventbus.UIEvents.OnBpInstalled -= ShowPanel;
    }
}