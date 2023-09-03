using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Clickable : MonoBehaviour
{
    public Tower _tower; //{ get; set; }

    private void OnEnable()
    {
        _tower = GetComponentInParent<Tower>();
    }

    void Setup(Tower tower)
    {
        _tower = tower;
    }

    private void OnMouseDown()
    {
        Eventbus.InputEvents.OnTowerPartClicked?.Invoke(_tower);
    }
}
