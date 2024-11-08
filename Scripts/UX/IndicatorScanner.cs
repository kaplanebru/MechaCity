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
        ShowTowerInfo();
    }

    void OnMouseExit()
    {
        if(!canHover) return;
        HideTowerInfo();
    }

    public void EnableHover(bool enable)
    {
        canHover = enable;
    }

    public void Setup(uint id)
    {
        ID = id;
    }
    void ShowTowerInfo()
    {
        GeneralEventbus.IndicatorEvents.OnActorHover?.Invoke(ID);
    }

    void HideTowerInfo()
    {
        //todo
    }
}
