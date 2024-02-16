using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BPInteraction : MonoBehaviour
{
    public Transform imageTransform;
    public Transform gear;
    
    private float startHeight;

    public Vector3 rot = new Vector3(0, 360, 0);
    private Vector3 startRot;

    public float hoverDuration = 1;
    public float selectDuration = 1;
    public float selectY = 0.1f;
    private bool onSlot = false;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        startHeight = transform.localPosition.y;
        startRot = gear.localEulerAngles;
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
        //imageTransform.DOLocalRotate(rot, duration, RotateMode.FastBeyond360);
        gear.DOLocalRotate(rot, hoverDuration, RotateMode.FastBeyond360);
    }

    void ResetImage()
    {
        gear.DOKill();
        gear.localEulerAngles = startRot;
        
        // imageTransform.DOKill();
        // imageTransform.localEulerAngles = startRot;
    }

    void Select()
    {
        //ResetImage();
        transform.DOLocalMoveY(selectY, selectDuration/2).OnComplete(() =>
        {
            transform.DOLocalMoveY(startHeight, selectDuration/2);
        });
    }
}
