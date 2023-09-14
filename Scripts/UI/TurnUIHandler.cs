using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnUIHandler : MonoBehaviour
{
    [SerializeField] private Button[] Buttons;
    private Button currentButton;
    private bool buttonFunctionCompleted = false;
    private bool hasSpecialCase = false;


    private void OnEnable() //ui daha önce gelmeli turnden
    {
        Buttons = GetComponentsInChildren<Button>();
        StartCoroutine(nameof(ButtonSequenceRoutine));
        Eventbus.TurnEvents.OnTurnEnded += RestartSequence;
        Eventbus.UIEvents.OnButtonCall += HandleSpecialCase;
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

    void HandleSpecialCase(bool enable)
    {
        hasSpecialCase = true;
        currentButton.gameObject.SetActive(enable);
    }

    void CompleteAndResetSequence()
    {
        currentButton.gameObject.SetActive(false);
        buttonFunctionCompleted = false;
        hasSpecialCase = false;
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
        Eventbus.TurnEvents.OnTurnEnded -= RestartSequence;
        Eventbus.UIEvents.OnButtonCall -= HandleSpecialCase;
    }
}