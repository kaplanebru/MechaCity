using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScanner : MonoBehaviour
{
    private uint ID;
    private bool canHover = false;
    void OnMouseEnter()
    {
        if(!canHover) return;
        ShowLines();
    }

    void OnMouseExit()
    {
        if(!canHover) return;
        HideLines();
    }

    public void EnableHover(bool enable)
    {
        canHover = enable;
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
