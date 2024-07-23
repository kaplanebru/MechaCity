using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Transform[] parts;
    public float decreasedHeight = 0.6f;
    public Transform gear;

    private float startHeight;
    private void OnEnable()
    {
        startHeight = parts[0].localScale.y;
    }

    void Decrease()
    {
        gear.gameObject.SetActive(true);
        
        foreach (var part in parts)
        {
            part.DOScaleY(decreasedHeight, 0.5f);
        }
    }

    void RestoreHeight()
    {
        gear.gameObject.SetActive(false);
        
        foreach (var part in parts)
        {
            part.DOScaleY(startHeight, 0.5f);
        }
    }

    private void OnDisable()
    {
        
    }
}
