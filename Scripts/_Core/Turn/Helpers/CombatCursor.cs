using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModels;
using DG.Tweening;
using Enums;
using Network;
using Teams;
using Towers;
using UnityEngine;

public class CombatCursor : MonoBehaviour
{
    
    public BPDataHolder bpDataHolder;
    public BpInstallEffect installEffect;
    public Transform gargouille;

    private CursorSpriteHandler cursorSpriteHandler;
    private SpriteRenderer spriteRenderer;
    private float _duration;
    
    public List<Transform> transforms;
    public List<Vector3> directions;
    public List<Vector3> targetPositions;

    private Vector3 center;
    public float distance = 1;

    
    private void OnEnable()
    {
        Eventbus.TeamEvents.OnTeamsSet += GetTransforms;
        Eventbus.CombatEvents.OnNextTower += ShiftTarget;

        Eventbus.CombatEvents.OnCombatStarted += StartCursor;
        Eventbus.CombatEvents.OnCombatEnding += EndCursor;

        Eventbus.StateEvents.OnLinkStateBegin += Swallow;
        
        BpEventbus.UIEvents.OnBpInstallBegin += SetupAndInstall;
        BpEventbus.UIEvents.OnBpReset += ResetBpImage;
        
        BpEventbus.SubscriberEvents.OnReverseAction += ReverseAngle;

        installEffect = GetComponentInChildren<BpInstallEffect>();
        installEffect.Initialize();
    }
    
    private void GetTransforms(Team[] obj)
    {
        for (int i = 0; i < AllTowers.TowersCount; i++)
        {
            transforms.Add(AllTowers.GetTower(i).transform);
        }
        Setup(); 
    }

    void SetReferences()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        cursorSpriteHandler = new CursorSpriteHandler(spriteRenderer);
    }

    void SetDirections()
    {
        foreach (var towerTransform in transforms)
        {
            var dir = (towerTransform.position - center).normalized;
            //directions.Add(dir);
            directions.Add(new Vector3(dir.x, 0, dir.z).normalized);
        }
    }
    void SetPositions()
    {
        foreach (var dir in directions)
        {
            targetPositions.Add(center + new Vector3(dir.x * distance, 0, dir.z * distance));
        }
    }
    void Setup()
    {
        SetReferences();
        SetDirections();
        center = transform.position;
        SetPositions();
    }

   
    private int index = 0;
    void ShiftTarget(float duration)
    {
        //transform.DOMove(targetPositions[index], duration); //.OnComplete(() => RotateGargouille(duration));
        RotateGargouille(duration);
        index++;
        index %= targetPositions.Count;
    }

    void RotateGargouille(float duration)
    {
        gargouille.transform.DORotateQuaternion(Quaternion.LookRotation(directions[index]), duration);
    }

    void ToCenter()
    {
        transform.DOMove(center, .3f);
        gargouille.transform.DORotate(Vector3.zero, 1f);
    }
    void StartCursor()
    {
       ToCenter();
    }

    void EndCursor()
    {
        ToCenter();
    }

    private void Swallow()
    {
        transform.DOMoveY(center.y - 1, 1);
    }
   
    void SetupAndInstall(BpType type)
    {
        var bpData = bpDataHolder.TypeDataPair[type];
        
        installEffect.ExecuteEffect(
            ()=> cursorSpriteHandler.SetBlueprintImage(bpData),
            ()=> BpEventbus.UIEvents.OnBpInstalled?.Invoke(type));
    }
    
    void ReverseAngle()
    {
        var first = targetPositions.First();
        targetPositions.Remove(first);
        targetPositions.Reverse();
        targetPositions.Insert(0, first);
    }
    public void ResetBpImage()
    {
        cursorSpriteHandler.ResetBpImage();
    }
    
    private void OnDisable()
    {
        Eventbus.TeamEvents.OnTeamsSet -= GetTransforms;
        Eventbus.CombatEvents.OnNextTower -= ShiftTarget;

        Eventbus.CombatEvents.OnCombatStarted -= StartCursor;
        Eventbus.CombatEvents.OnCombatEnding -= EndCursor;
        Eventbus.StateEvents.OnLinkStateBegin -= Swallow;
        
        BpEventbus.UIEvents.OnBpInstallBegin -= SetupAndInstall;
        BpEventbus.UIEvents.OnBpReset -= ResetBpImage;
        
        BpEventbus.SubscriberEvents.OnReverseAction -= ReverseAngle;
    }
}