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
    private float _duration = 0.7f;

    private void OnEnable()
    {
        Eventbus.CombatEvents.OnFire += Loop;
    }

    void Start()
    {
        Setup(); //initializera al

    }

    void Setup()
    {
        _currentAngle = transform.eulerAngles.x;
        intervalAngle = 360f / AllTowers.TowersCount;
    }
    
    void Loop(float duration)
    {
        //_duration = duration;
        StartCoroutine(nameof(LoopRoutine));
    }

    IEnumerator LoopRoutine()
    {
        for (int i = 0; i < AllTowers.TowersCount; i++)
        {
            ShiftTarget();
            _currentAngle = (_currentAngle + intervalAngle) % 360;
            yield return transform.DORotate(new Vector3(0, _currentAngle, 0), _duration).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    void ShiftTarget()
    {
       
         //, RotateMode.FastBeyond360
    }

    private void OnDisable()
    {
        Eventbus.CombatEvents.OnFire -= Loop;
    }
}
