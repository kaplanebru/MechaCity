using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BPInteraction : MonoBehaviour
{
    public Transform imageTransform;
    
    Vector3 startScale; // = Vector3.one;
    private float startHeight;

    private Vector3 rot = new Vector3(0, 360, 0);
    private Vector3 startRot;
    
    public float scaleAmount = 1.5f;
    public float duration = 1;
    public float selectY = 0.1f;
    private bool onSlot = false;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        startScale = imageTransform.localScale;
        startHeight = transform.localPosition.y;
        startRot = imageTransform.localEulerAngles;
    }
    private void OnMouseEnter()
    {
        HoverImage();
    }

    private void OnMouseDown()
    {
        Select();
    }

    private void OnMouseExit()
    {
        ResetImage();
    }

    void HoverImage()
    {
        // imageTransform.DOScale(new Vector3(scaleAmount, startScale.y, scaleAmount), duration/2).OnComplete(() =>
        // {
        //     imageTransform.DOScale(startScale, duration / 2);
        // });
        imageTransform.DOLocalRotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360);
    }

    void ResetImage()
    {
        imageTransform.DOKill();
        //imageTransform.localScale = startScale;
        imageTransform.localEulerAngles = startRot;
    }

    void Select()
    {
        //ResetImage();
        transform.DOLocalMoveY(selectY, duration/2).OnComplete(() =>
        {
            transform.DOLocalMoveY(startHeight, duration/2);
        });
    }
}
