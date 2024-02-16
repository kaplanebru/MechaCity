using System.Collections;
using System.Collections.Generic;
using DataModels;
using UnityEngine;

public class BPSlot : MonoBehaviour
{
    public BlueprintData Data;
    public SpriteRenderer spriteHolder;
    

    public void Setup(BlueprintData data)
    {
        Data = data;
        SetImage();
    }

    void SetImage()
    {
        if (spriteHolder == null) return;
        spriteHolder.sprite = Data.Sprite;
    }

  

    void SetColor()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Data.Color;
    }
}
