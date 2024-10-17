using System.Collections.Generic;
using System.Linq;
using DataModels;
using DG.Tweening;
using Enums;
using UnityEngine;

public class CombatCursor : MonoBehaviour
{
    
    public BPDataHolder bpDataHolder;
    public BpInstallEffect installEffect;
    public Transform cursorObj;
    public List<Transform> transforms;
    public float distance = 1;

    private CursorSpriteHandler cursorSpriteHandler;
    private SpriteRenderer spriteRenderer;
    private float _duration;
    
    public List<Vector3> directions = new();
    public List<Vector3> targetPositions = new();

    private Vector3 center;
    

    
    private void OnEnable()
    {
        GeneralEventbus.InitializerEvents.OnTowersCreated += GetTransforms;
        Eventbus.CombatEvents.OnNextActor += ShiftTarget;

        Eventbus.CombatEvents.OnCombatStarted += StartCursor;
        Eventbus.CombatEvents.OnCombatEnding += EndCursor;

        Eventbus.LinkEvents.OnLinkStateBegin += Swallow;
        
        BpEventbus.UIEvents.OnBpInstallBegin += SetupAndInstall;
        BpEventbus.UIEvents.OnBpReset += ResetBpImage;
        
        BpEventbus.SubscriberEvents.OnReverseAction += ReverseAngle;

        installEffect = GetComponentInChildren<BpInstallEffect>();
        installEffect.Initialize();
    }
    
    private void GetTransforms()
    {
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
        center = cursorObj.transform.position;
        SetReferences();
        SetDirections();
        SetPositions();
    }

   
    private int index = 0;
    void ShiftTarget(float duration)
    {
        cursorObj.transform.DOMove(targetPositions[index], duration); //.OnComplete(() => RotateGargouille(duration));
        RotateCursorObj(duration);
        index++;
        index %= targetPositions.Count;
    }

    void RotateCursorObj(float duration)
    {
        cursorObj.transform.DORotateQuaternion(Quaternion.LookRotation(directions[index]), duration);
    }

    void ToCenter()
    {
        cursorObj.transform.DOMove(center, .3f);
        cursorObj.transform.DORotate(Vector3.zero, 1f);
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
        cursorObj.transform.DOMoveY(center.y - 1, 1);
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
        GeneralEventbus.InitializerEvents.OnTowersCreated -= GetTransforms;
        Eventbus.CombatEvents.OnNextActor -= ShiftTarget;

        Eventbus.CombatEvents.OnCombatStarted -= StartCursor;
        Eventbus.CombatEvents.OnCombatEnding -= EndCursor;
        Eventbus.LinkEvents.OnLinkStateBegin -= Swallow;
        
        BpEventbus.UIEvents.OnBpInstallBegin -= SetupAndInstall;
        BpEventbus.UIEvents.OnBpReset -= ResetBpImage;
        
        BpEventbus.SubscriberEvents.OnReverseAction -= ReverseAngle;
    }
}