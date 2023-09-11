using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;
using UnityEngine.EventSystems;


public class Clickable : BaseClickable<Tower>
{
    public int id; //for multiplayer
    public TeamType teamType;
    protected override void Setup()
    {
        clickableObject = GetComponentInParent<Tower>();
    }
}
