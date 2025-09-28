using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class DoubleInteraction : MonoBehaviour //TEMPORARY
{
    public void OnClick()
    {
        BpEventbus.SelectionEvents.OnBpSlotSelected?.Invoke(BpType.DoubleSelf, 1);
    }
}
