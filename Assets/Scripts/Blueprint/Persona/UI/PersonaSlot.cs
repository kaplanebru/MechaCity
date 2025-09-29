using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class PersonaSlot : MonoBehaviour
{
    public PersonaType Type { get; private set; }
    public Image imageHolder;
    public Image canceledImageHolder;
    public Button button;

    public void Setup(PersonaSlotData data)
    {
        Type = data.Type;
        imageHolder.sprite = data.Sprite;
    }

    public void OnClick()
    {
        button.enabled = false;
        BpEventbus.PersonaEvents.OnPersonaSlotClicked?.Invoke(Type);
    }

    public void Cancel()
    {
        button.interactable = false;
        canceledImageHolder.gameObject.SetActive(true);
    }
}