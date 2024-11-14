using System.Collections.Generic;
using System.Linq;
using Actor;
using DataModels;
using DG.Tweening;
using Enums;
using UnityEngine;

public class CombatCursor : MonoBehaviour
{
    
    public BPDataHolder bpDataHolder;
    public BpInstallEffect installEffect;
    public Transform cursorObj;
    
    public float distance = 1;

    private CursorSpriteHandler cursorSpriteHandler;
    private SpriteRenderer spriteRenderer;
    private float _duration;
    
    public List<Vector3> positions = new();
    public List<Vector3> directions = new();
    public List<Vector3> targetPositions = new();
    private Vector3 center;
    
    private void OnEnable()
    {
        Eventbus.CombatEvents.OnPairsSet += Setup;

        Eventbus.CombatEvents.OnNextActor += ShiftTarget;

        Eventbus.CombatEvents.OnCombatStarted += StartCursor;
        Eventbus.CombatEvents.OnCombatEnding += EndCursor;

        Eventbus.TurnStateEvents.OnTurnStateBegin += Swallow;
        
        BpEventbus.UIEvents.OnBpInstallBegin += SetupAndInstall;
        BpEventbus.UIEvents.OnBpReset += ResetBpImage;
        
        
        SetVisualReferences();
        installEffect = GetComponentInChildren<BpInstallEffect>();
        installEffect.Initialize();
        center = cursorObj.transform.position;
    }
    
   
    void Setup(bool isReversed)
    {
        FillActorPositions();
        SetDirections();
        SetTargetPositions();
        if(isReversed)
            ReverseAngle();
    }

    void FillActorPositions()
    {
        positions.Clear();
        foreach (var actor in  ActorHolder.Registry.Values)
        {
            positions.Add(actor.Center);
        }
    }

    void SetDirections()
    {
        directions.Clear();
        foreach (var actorPos in positions)
        {
            var dir = (actorPos - center).normalized;
            directions.Add(new Vector3(dir.x, 0, dir.z).normalized);
        }
    }
    void SetTargetPositions()
    {
        targetPositions.Clear();
        foreach (var dir in directions)
        {
            targetPositions.Add(center + new Vector3(dir.x * distance, 0, dir.z * distance));
        }
    }

    void SetVisualReferences()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        cursorSpriteHandler = new CursorSpriteHandler(spriteRenderer);
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

    private void Swallow(TurnStateType turnState)
    {
        if(turnState == TurnStateType.Link)
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
        targetPositions.Reverse();
    }
    public void ResetBpImage()
    {
        cursorSpriteHandler.ResetBpImage();
    }
    
    private void OnDisable()
    {
        Eventbus.CombatEvents.OnPairsSet -= Setup;
        Eventbus.CombatEvents.OnNextActor -= ShiftTarget;

        Eventbus.CombatEvents.OnCombatStarted -= StartCursor;
        Eventbus.CombatEvents.OnCombatEnding -= EndCursor;
        Eventbus.TurnStateEvents.OnTurnStateBegin -= Swallow;
        
        BpEventbus.UIEvents.OnBpInstallBegin -= SetupAndInstall;
        BpEventbus.UIEvents.OnBpReset -= ResetBpImage;
        
        
    }
}