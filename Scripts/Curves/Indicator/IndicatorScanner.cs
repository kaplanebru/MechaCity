using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScanner : MonoBehaviour
{
    private uint ID;
    void OnMouseEnter()
    {
        ShowLines();
    }

    void OnMouseExit()
    {
        HideLines();
    }

    public void Setup(uint id)
    {
        ID = id;
    }
    void ShowLines()
    {
        GeneralEventbus.IndicatorEvents.OnActorHover?.Invoke(ID);
    }

    void HideLines()
    {
        GeneralEventbus.IndicatorEvents.OnLeavingActor?.Invoke();
    }
}
