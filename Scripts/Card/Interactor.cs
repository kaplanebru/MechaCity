using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private TransformValues transformValues;
    public float scaleAmount = 1.5f;
    public float duration = 1;
    public float endY = 0.1f;

    private void Start()
    {
        transformValues = new TransformValues(transform);
    }

    private void OnMouseEnter()
    {
        KillTweens();
        Vector3 newScale = new Vector3(1, transformValues.startScale.y, 1) * scaleAmount;
        transform.DOScale(newScale, duration).SetId("Scale");
        transform.DORotate(Vector3.zero, duration).SetId("Rotate");
        transform.DOLocalMoveY(endY, duration).SetId("Move");
    }

    private void OnMouseExit()
    {
        KillTweens();
        transformValues.ResetValues();
    }

    private void OnMouseDown()
    {
        //ayrı bir bölgeye gitsin
        //card deck update olsun
    }

    void KillTweens()
    {
        DOTween.Kill("Scale"); //bekle sonra kill et
        DOTween.Kill("Rotate");
        DOTween.Kill("Move");
    }
}


public class TransformValues
{
    public Vector3 startScale;
    public Vector3 startPos;
    public Quaternion startRot;

    private Transform _transform;

    public TransformValues(Transform transform)
    {
        _transform = transform;
        GetValues();
    }

    void GetValues()
    {
        startScale = _transform.localScale;
        startPos = _transform.position;
        startRot = _transform.rotation;
    }

    public void ResetValues()
    {
        _transform.position = startPos;
        _transform.rotation = startRot;
        _transform.localScale = startScale;
    }
}