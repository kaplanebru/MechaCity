using System.Collections;
using System.Collections.Generic;
using DataModels;
using TMPro;
using UnityEngine;

public class BPSlot : MonoBehaviour
{
    public BlueprintData Data;
    public SpriteRenderer spriteHolder;

    public TextMeshPro titleHolder;
    public TextMeshPro descriptionHolder;
    

    public void Setup(BlueprintData data)
    {
        Data = data;
        SetImage();
        SetTexts();
    }

    void SetImage()
    {
        if (spriteHolder == null) return;
        spriteHolder.sprite = Data.Sprite;
    }


    void SetTexts()
    {
        titleHolder.text = Data.Title;
        descriptionHolder.text = Data.Description;
    }

    void SetColor()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Data.Color;
    }
}
