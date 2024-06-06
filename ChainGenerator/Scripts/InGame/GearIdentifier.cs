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

    private void OnEnable()
    {
        _yAngle = transform.localEulerAngles.y;
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
        _yAngle += rotateAngle;
        _yAngle %= 360;
        Quaternion newRot = Quaternion.Euler(0, _yAngle, 0);
        transform.DOLocalRotateQuaternion(newRot, 1.1f);
    }

    private void OnDisable()
    {
        CommunEventbus.ChainTurnEvents.OnInitialize -= Rotate;
        CommunEventbus.EffectEvents.OnDeathEffect -= IndividualRotate;
    }
}
