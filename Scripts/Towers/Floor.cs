using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Floor : MonoBehaviour, ITowerRelated
{
    public Transform[] parts;
    public Transform gear;

    private float startHeight;
    private void OnEnable()
    {
        startHeight = parts[0].localScale.y;
    }

    public void Open(float height, float duration)
    {
        gear.gameObject.SetActive(true);
        
        foreach (var part in parts)
        {
            part.DOScaleY(height, duration);
        }
    }

    public void RestoreHeight(float duration)
    {
        gear.gameObject.SetActive(false);
        
        foreach (var part in parts)
        {
            part.DOScaleY(startHeight, duration);
        }
    }

    public int Id { get; set; }
    public void Initialize(int id)
    {
        Id = id;
    }
}
