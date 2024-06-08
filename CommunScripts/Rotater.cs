using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Rotater
{
    private Transform _transform;
    private Vector3 _rotateAmount;

    public Rotater(Transform transform)
    {
        _transform = transform;
    }

    public void Rotate(float rotateAngle)
    {
        _rotateAmount = new Vector3(0, rotateAngle, 0);
        Vector3 target = _transform.localEulerAngles + _rotateAmount;
        _transform.DOLocalRotate(target, 1.1f, RotateMode.FastBeyond360);
    }
}