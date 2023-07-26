using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SequentialButtons : MonoBehaviour
{
    [SerializeField] private Button[] Buttons;
    private bool buttonDisabled = false;

    private void Start()
    {
        Buttons = GetComponentsInChildren<Button>();
        StartCoroutine(nameof(ButtonSequenceRoutine));
    }

    public IEnumerator ButtonSequenceRoutine()
    {
        DisableAllButtons();
        foreach (var button in Buttons)
        {
            button.gameObject.SetActive(true);

            yield return new WaitUntil(() => buttonDisabled);

            button.gameObject.SetActive(false);
            buttonDisabled = false;
        }
    }

    public void ButtonDisabled() => buttonDisabled = true;

    void DisableAllButtons()
    {
        foreach (var button in Buttons)
        {
            button.gameObject.SetActive(false);
        }
    }
}