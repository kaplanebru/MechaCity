using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RotationHelper
{
    private float _rotateAngle;
    private Transform _transform;

    public RotationHelper(Transform transform, float rotateAngle)
    {
        _transform = transform;
        _rotateAngle = rotateAngle;
    }

    public void Rotate()
    {
        Vector3 incrementEulerAngles = new Vector3(0, _rotateAngle, 0);
        
       Quaternion target = _transform.localRotation * Quaternion.Euler(incrementEulerAngles);
       _transform.DOLocalRotateQuaternion(target, 1.1f);
    }
}