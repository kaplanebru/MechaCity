using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BPInteractor : MonoBehaviour
{
    public Camera bpCam;
    private Ray ray;
    private GameObject currentGO = null;
    private GameObject selectableGO = null;
    
    Vector3 startScale; // = Vector3.one;
    public float scaleAmount = 1.5f;
    public float duration = 1;

    private bool onSlot = false;

    private void Start()
    {
        StartCoroutine(nameof(InteractRoutine));
    }

    IEnumerator InteractRoutine()
    {
        while (true)
        {
            ray = bpCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))//, LayerMask.GetMask("BlueprintSlot")))
            {
              
                print("bp");

                if (hit.transform.gameObject != currentGO ||
                    (hit.transform.gameObject == currentGO && !onSlot)) //hit.transform.gameObject != currentGO)
                {
                    Reset();
                    currentGO = hit.transform.gameObject;
                    Hover();
                }
            }
            else
            {
                print("empty");
                Reset();
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private bool isFirst = true;
    void Hover()
    {
        onSlot = true;
        if (isFirst)
        {
            startScale = currentGO.transform.localScale;
            isFirst = false;
        }
        currentGO.transform.DOScale(new Vector3(scaleAmount, 1, scaleAmount), duration);
    }

    void Reset()
    {
        onSlot = false;
        if(currentGO == null) return;
        currentGO.transform.DOKill();
        currentGO.transform.localScale = startScale;
    }
}