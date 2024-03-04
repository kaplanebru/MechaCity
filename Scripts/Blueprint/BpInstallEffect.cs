using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BpInstallEffect :MonoBehaviour
{
   public Transform partHolder;
   public Transform[] parts;

   private float partStartSize;
   public float partEndSize;
   public float stretchDuration;
   
   public Vector3 rotationAngle;
   private Vector3 partStartRotation;
   public float rotationDuration;
   
   private RotationEffect _rotationEffect;
   

   public void Initialize()
   {
      partStartSize = parts[0].transform.localScale.z;
      partStartRotation = partHolder.localEulerAngles;
      
      _rotationEffect = new RotationEffect(partHolder, rotationAngle, rotationDuration, partStartRotation);
   }


   public void ExecuteEffect()
   {
      StartCoroutine(nameof(EffectRoutine));
   }

   IEnumerator EffectRoutine()
   {
      Stretch(true);
      yield return new WaitForSeconds(stretchDuration);
      
      _rotationEffect.ExecuteRotation();
      yield return new WaitForSeconds(rotationDuration);

      Stretch(false);
      _rotationEffect.ResetRotation();
   }

   void Stretch(bool stretchUp)
   {
      float endValue = stretchUp ? partEndSize : partStartSize;
   
      foreach (var part in parts)
      {
         part.DOScaleZ(endValue, stretchDuration);
      }
   }
   
   
   
}
