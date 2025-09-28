using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RotationEffect
{
    private Transform _target;
    private Vector3 _rotationAngle;
    private float _duration;
    private Vector3 _startRotation;
    
    public RotationEffect(Transform target, Vector3 rotationAngle, float duration, Vector3 startRotation)
    {
        _target = target;
        _rotationAngle = rotationAngle;
        _duration = duration;
        _startRotation = startRotation;
    }

    public void ExecuteRotation()
    {
        _target.DOLocalRotate(_rotationAngle, _duration, RotateMode.FastBeyond360);
    }

    public void ResetRotation()
    {
        _target.DOKill();
        _target.localEulerAngles = _startRotation;
    }
}
