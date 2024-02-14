using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Towers;
using UnityEngine;

public class CombatCursor : MonoBehaviour
{
    public float _currentAngle;
    public float intervalAngle;
    private float _duration;
    public GameObject line;

    private void OnEnable()
    {
        Eventbus.CombatEvents.OnFire += ShiftTarget; 
        Eventbus.CombatEvents.OnCombatStarted += EnableLine;
        Eventbus.CombatEvents.OnCombatEnding += DisableLine;
    }

    void Start()
    {
        Setup(); //initializera al
        DisableLine();
    }

    void Setup()
    {
        _currentAngle = transform.eulerAngles.x;
        intervalAngle = 360f / AllTowers.TowersCount;
    }
    
  
    
    void ShiftTarget(float duration)
    {
        DisableLine();
        _currentAngle = (_currentAngle + intervalAngle) % 360;
         transform.DORotate(new Vector3(0, _currentAngle, 0), duration).OnComplete(EnableLine);
    }

    void EnableLine()
    {
        line.SetActive(true);
    }

    void DisableLine()
    {
        line.SetActive(false);
    }
    
    

    private void OnDisable()
    {
        Eventbus.CombatEvents.OnFire -= ShiftTarget;
        Eventbus.CombatEvents.OnCombatStarted -= EnableLine;
        Eventbus.CombatEvents.OnCombatEnding -= DisableLine;
    }
}
