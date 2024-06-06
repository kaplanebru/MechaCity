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
    public float rotateAngle = 90;
    private float _yAngle;
    private RotationHelper rotater;

    private void OnEnable()
    {
        rotater = new RotationHelper(transform, 90);
        
        CommunEventbus.ChainTurnEvents.OnInitialize += Rotate;
        CommunEventbus.EffectEvents.OnDeathEffect += IndividualRotate;
    }

    void IndividualRotate(int Id) //tüm kule dönecekse buna gerek yok ayrıca
    {
        if(id != Id) return;
        Rotate();
    }

    void Rotate()
    {
        rotater.Rotate();
    }

    private void OnDisable()
    {
        CommunEventbus.ChainTurnEvents.OnInitialize -= Rotate;
        CommunEventbus.EffectEvents.OnDeathEffect -= IndividualRotate;
    }
}
