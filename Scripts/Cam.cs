using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DataModels;
using DG.Tweening;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform combatTransform;
    public CombatTimingData timingData;
    public Ease ease;
    private Vector3 startPos;
    private Quaternion startRot;

    private void OnEnable()
    {
        Eventbus.CombatEvents.OnCombatReady += SwitchToCombatCam;
        Eventbus.CombatEvents.OnCombatTerminated += ResetCam;
    }

    private void Start()
    {
        Setup();
    }

    void Setup()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void SwitchToCombatCam()
    {
        Move(combatTransform.position, combatTransform.rotation);
    }

    void ResetCam()
    {
        Move(startPos, startRot);
    }

    void Move(Vector3 pos, Quaternion rot)
    {
        transform.DOMove(pos, timingData.cameraDelay).SetEase(ease);
        transform.DORotateQuaternion(rot, timingData.cameraDelay).SetEase(ease);
    }

    private void OnDisable()
    {
        Eventbus.CombatEvents.OnCombatReady -= SwitchToCombatCam;
        Eventbus.CombatEvents.OnCombatTerminated -= ResetCam;
    }
}
