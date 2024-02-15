using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardInteractor : MonoBehaviour
{
    public Camera cardCam;
    private Ray ray;
    private GameObject currentGO = null;


    private Quaternion startRot;
    private Vector3 startPos;
    
    public float scaleAmount = 1.5f;
    public float duration = 1;
    public float endY = 0.1f;
    private Vector3 newScale;
    private bool onCard = false;
    void Start()
    {
       
        StartCoroutine(nameof(InteractRoutine));
    }

    
    IEnumerator InteractRoutine()
    {
        while (true)
        {
            ray = cardCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Card")))
            {
                if (currentGO == null || hit.transform.gameObject != currentGO)
                {
                    Reset();
                    currentGO = hit.transform.gameObject;
                    Hover();
                }
            }
            else
            {
               
                if(!onCard)
                    Reset();
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void Hover()
    {
        onCard = true;
        startRot = currentGO.transform.localRotation;
        startPos = currentGO.transform.localPosition;
        currentGO.transform.DOScale(new Vector3(scaleAmount, 1, scaleAmount), duration);
        currentGO.transform.DOLocalMoveY(endY, duration);
        currentGO.transform.DOLocalRotate(Vector3.zero, duration).OnComplete(() =>
        {
            onCard = false;
        });
    }

    private void Reset()
    {
        if (currentGO == null) return;
        currentGO.transform.DOKill();
        currentGO.transform.localScale = Vector3.one;
        currentGO.transform.localRotation = startRot;
        currentGO.transform.localPosition = startPos;
    }
}
