using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Clickable : BaseClickable<Tower>
{
    protected override void Setup()
    {
        clickableObject = GetComponentInParent<Tower>();
    }
}
