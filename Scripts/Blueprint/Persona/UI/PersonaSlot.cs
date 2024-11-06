using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class PersonaSlot : MonoBehaviour
{
   private PersonaType type;
   public Image imageHolder;
   public Button button;

   public void Setup(PersonaSlotData data)
   {
      type = data.Type;
      imageHolder.sprite = data.Sprite;
   }

   public void OnClick()
   {
      BpEventbus.PersonaEvents.OnPersonaSlotClicked?.Invoke(type);
   }
}
