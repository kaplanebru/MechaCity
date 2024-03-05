using System.Collections;
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


    private void OnEnable() //ui daha önce gelmeli turnden
    {
      
        UIEventbus.TurnEvents.OnInitialize += Initialize;
        
    }

    private void Initialize()
    {
       // Buttons = GetComponentsInChildren<Button>(); //ownerın butonlarını da alıyor 0 olarak, kendi butonlarından sonra!
        DisableAllButtons();
        
        UIEventbus.TurnEvents.OnTurnButtonsShiftRequest += RestartSequence;
        UIEventbus.OnButtonCall += HandleSpecialCase;
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
        if(currentButton == null) return;
        
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

        //Eventbus.TurnEvents.OnTurnEnding -= RestartSequence;
        UIEventbus.OnButtonCall -= HandleSpecialCase;
    }
}
}
