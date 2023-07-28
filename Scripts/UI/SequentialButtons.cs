using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SequentialButtons : MonoBehaviour
{
    [SerializeField] private Button[] Buttons;
    private Button currentButton;
    private bool buttonFunctionCompleted = false;
    private bool hasSpecialCase = false;
    

    private void Start()
    {
        Buttons = GetComponentsInChildren<Button>();
        Eventbus.UIEvents.OnButtonCall += HandleButton;

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

            CompleteSequence();
        }
    }

    void HandleButton(bool enable)
    {
        hasSpecialCase = true;
        currentButton.gameObject.SetActive(enable);
    }

    void CompleteSequence()
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
        Eventbus.UIEvents.OnButtonCall -= HandleButton;
    }
}