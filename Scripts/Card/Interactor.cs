using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    // private TransformValues transformValues;
    // public float scaleAmount = 1.5f;
    // public float duration = 1;
    // public float endY = 0.1f;
    //
    // [SerializeField] private bool onHover = false;
    // private Vector3 newScale;
    //
    // private void Start()
    // {
    //     transformValues = new TransformValues(transform);
    //     newScale = new Vector3(1, transformValues.startScale.y, 1) * scaleAmount;
    // }
    //
    //
    // // private void OnMouseEnter()
    // // {
    // //     if (!onHover)
    // //     {
    // //         onHover = true;
    // //         Hover();
    // //     }
    // // }
    // //
    // // private void OnMouseExit()
    // // {
    // //     // if (onHover)
    // //     // {
    // //     //     onHover = false;
    // //     //     Reset();
    // //     // }
    // //     onHover = false;
    // //     transform.DOKill();
    // //     Reset();
    // // }
    //
    // void Hover()
    // {
    //     transform.DOKill();
    //     Adjust(newScale, Vector3.zero, endY);
    // }
    //
    // void Reset()
    // {
    //     // KillTweens();
    //     // Adjust(transformValues.startScale, transformValues.startRot.eulerAngles, transformValues.startPos.y);
    //     transformValues.ResetValues();
    //     onHover = false;
    // }
    //
    // void Adjust(Vector3 scale, Vector3 rot, float y)
    // {
    //     transform.DOScale(scale, duration); 
    //     transform.DOLocalRotate(rot, duration); 
    //     transform.DOLocalMoveY(y, duration).OnComplete(Reset);
    // }
    //
    // void KillTweens()
    // {
    //     transform.DOKill();
    //     // DOTween.Kill("Scale"); //bekle sonra kill et
    //     // DOTween.Kill("Rotate");
    //     // DOTween.Kill("Move");
    // }
    //
    //
    // private void OnMouseDown()
    // {
    //     //ayrı bir bölgeye gitsin
    //     //card deck update olsun
    // }
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
        startPos = _transform.localPosition;
        startRot = _transform.localRotation;
    }

    public void ResetValues()
    {
        _transform.localPosition = startPos;
        _transform.localRotation = startRot;
        _transform.localScale = startScale;
    }
}