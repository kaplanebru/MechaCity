using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using DG.Tweening;
using UnityEngine;

public class GearIdentifier : MonoBehaviour //temp
{
    //public GearData gearData; //hepsi aynı geardata, burdan olmaz
    public int id;
    private Rotater _rotater;

    private void OnEnable()
    {
        _rotater = new Rotater(transform);
        
        CommunEventbus.ChainTurnEvents.OnInitialize += Rotate;
        CommunEventbus.EffectEvents.OnDeathEffect += IndividualRotate;
    }

    void IndividualRotate(int Id)
    {
        if(id != Id) return;
        _rotater.Rotate(360);
    }

    void Rotate()
    {
        _rotater.Rotate(90);
    }

    private void OnDisable()
    {
        CommunEventbus.ChainTurnEvents.OnInitialize -= Rotate;
        CommunEventbus.EffectEvents.OnDeathEffect -= IndividualRotate;
    }
}
