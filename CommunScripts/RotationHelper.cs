using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RotationHelper
{
    private Transform _transform;
    private Vector3 _incrementEulerAngles;

    public RotationHelper(Transform transform, float rotateAngle)
    {
        _transform = transform;
        _incrementEulerAngles = new Vector3(0, rotateAngle, 0);
    }

    public void Rotate()
    {
        Vector3 target = _transform.localEulerAngles + _incrementEulerAngles;
        _transform.DOLocalRotate(target, 1.1f, RotateMode.FastBeyond360);
    }
}