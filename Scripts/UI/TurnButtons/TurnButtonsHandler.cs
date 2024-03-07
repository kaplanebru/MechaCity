using System.Collections;
using System.Collections.Generic;
using DataModels;
using Enums;
using Network;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    
    public class TurnButtonsHandler : MonoBehaviour
{
    [SerializeField] private Button[] Buttons;
    private Button currentButton;
    private bool buttonFunctionCompleted = false;
    private bool hasSpecialCase = false;

    public TurnButtonHolder buttonHolder;

    private void OnEnable() //ui daha önce gelmeli turnden
    {
        UIEventbus.TurnEvents.OnInitialize += Initialize;
    }

    private void Initialize()
    {
       // Buttons = GetComponentsInChildren<Button>(); //ownerın butonlarını da alıyor 0 olarak, kendi butonlarından sonra!
        DisableAllButtons();
        
        UIEventbus.TurnEvents.OnTurnButtonsShiftRequest += RestartSequence;
        UIEventbus.OnButtonCall += ShowButton;
    }
    

    private void RestartSequence()
    {
        StopCoroutine(nameof(ButtonSequenceRoutine));
        StartCoroutine(nameof(ButtonSequenceRoutine));
    }


    public IEnumerator ButtonSequenceRoutine()
    {
        DisableAllButtons();
        foreach (var button in Buttons)
        {
            currentButton = button;
            
            if(!hasSpecialCase)
                button.gameObject.SetActive(true);
            
            yield return new WaitUntil(() => buttonFunctionCompleted);
            CompleteAndResetSequence();
        }
    }

    void ShowButton(bool enable, TurnStateType type)
    {
        if(currentButton == null) return;
        
        print("show button on type: " + type);
        hasSpecialCase = true;
        currentButton.gameObject.SetActive(enable);
    }

    void CompleteAndResetSequence()
    {
        currentButton.gameObject.SetActive(false);
        buttonFunctionCompleted = false;
        hasSpecialCase = false;
        currentButton = null;
    }

    public void ButtonDisabled() => buttonFunctionCompleted = true;

    void DisableAllButtons()
    {
        foreach (var button in Buttons)
        {
            button.gameObject.SetActive(false);
        }
    }
    
    private void OnDisable()
    {
        UIEventbus.TurnEvents.OnInitialize -= Initialize;
        UIEventbus.TurnEvents.OnTurnButtonsShiftRequest -= RestartSequence;

        UIEventbus.OnButtonCall -= ShowButton;
    }
}
}
