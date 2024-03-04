using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using DG.Tweening;
using Enums;
using Network;
using Towers;
using UnityEngine;

public class CombatCursor : MonoBehaviour
{
    public float _currentAngle;
    public float intervalAngle;
    public GameObject line;
    public GameObject cursor;
    public BPDataHolder bpDataHolder;
    public BpInstallEffect installEffect;

    private CursorBpHandler cursorBpHandler;
    private SpriteRenderer spriteRenderer;
    private float _duration;

    
    private void OnEnable()
    {
        Eventbus.CombatEvents.OnFire += ShiftTarget;

        Eventbus.CombatEvents.OnCombatStarted += EnableLine;
        Eventbus.CombatEvents.OnCombatEnding += DisableLine;

        Eventbus.CombatEvents.OnCombatStarted += EnableCursor;
        Eventbus.CombatEvents.OnCombatEnding += DisableCursor;

        NetworkEventbus.BlueprintEvents.OnBpSelected += SetBpImage;
        NetworkEventbus.RequestEvents.OnNewTurnRequest += ResetBp;
        BpEventbus.SubscriberEvents.OnReverseAction += ReverseAngle;

        installEffect = GetComponentInChildren<BpInstallEffect>();
        installEffect.Initialize();
    }

    void SetReferences()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        cursorBpHandler = new CursorBpHandler(spriteRenderer);
    }

    void Start()
    {
        Setup(); //initializera al
        DisableLine();
    }

    void Setup()
    {
        SetReferences();

        _currentAngle = transform.eulerAngles.x;
        intervalAngle = 360f / AllTowers.TowersCount;
    }

    void ReverseAngle()
    {
        intervalAngle *= -1;
    }

    void ShiftTarget(float duration)
    {
        DisableLine();

        _currentAngle = (_currentAngle + intervalAngle) % 360;
        transform.DORotate(new Vector3(0, _currentAngle, 0), duration).OnComplete(EnableLine);
    }

    void SetBpImage(BpType type)
    {
        var bpData = bpDataHolder.TypeDataPair[type];
        
        cursorBpHandler.SetBlueprintImage(bpData);
        installEffect.ExecuteEffect();
    }

    void ResetBp()
    {
        cursorBpHandler.Reset();
    }

    void EnableLine()
    {
        line.SetActive(true);
    }

    void DisableLine()
    {
        line.SetActive(false);
    }

    void EnableCursor()
    {
        cursor.SetActive(true);
    }

    void DisableCursor()
    {
        cursor.SetActive(false);
    }


    private void OnDisable()
    {
        Eventbus.CombatEvents.OnFire -= ShiftTarget;

        Eventbus.CombatEvents.OnCombatStarted -= EnableLine;
        Eventbus.CombatEvents.OnCombatEnding -= DisableLine;

        Eventbus.CombatEvents.OnCombatStarted -= EnableCursor;
        Eventbus.CombatEvents.OnCombatEnding -= DisableCursor;
        
        NetworkEventbus.BlueprintEvents.OnBpSelected -= SetBpImage;
        NetworkEventbus.RequestEvents.OnNewTurnRequest -= ResetBp;
        
        BpEventbus.SubscriberEvents.OnReverseAction -= ReverseAngle;
    }
}