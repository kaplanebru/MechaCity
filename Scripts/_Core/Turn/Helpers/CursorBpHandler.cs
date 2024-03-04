using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using UnityEngine;
using DG.Tweening;

public class CursorBpHandler 
{
   private SpriteRenderer _spriteRenderer;
   private BlueprintData currentBpData;
   private Transform _spriteTransform;
   private Vector3 startSize;

   public CursorBpHandler(SpriteRenderer spriteRenderer)
   {
      _spriteRenderer = spriteRenderer;
      
      _spriteTransform = _spriteRenderer.GetComponent<Transform>();
      startSize = _spriteTransform.localScale;
   }
  

   public void SetBlueprintImage(BlueprintData data)
   {
      currentBpData = data;
      
      _spriteTransform.localScale = Vector3.zero;
      _spriteRenderer.sprite = currentBpData.Sprite;
      _spriteTransform.DOScale(startSize, .4f);
   }
   
  

   public void Reset()
   {
      currentBpData = null;
      _spriteRenderer.sprite = null;
   }
}
