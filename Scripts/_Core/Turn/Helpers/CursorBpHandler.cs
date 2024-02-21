using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using UnityEngine;

public class CursorBpHandler 
{
   private SpriteRenderer _spriteRenderer;
   private BlueprintData currentBpData;

   public CursorBpHandler(SpriteRenderer spriteRenderer)
   {
      _spriteRenderer = spriteRenderer;
   }
  

   public void SetBlueprintImage(BlueprintData data)
   {
      currentBpData = data;
      _spriteRenderer.sprite = currentBpData.Sprite;
   }

   public void Reset()
   {
      currentBpData = null;
      _spriteRenderer.sprite = null;
   }
}
