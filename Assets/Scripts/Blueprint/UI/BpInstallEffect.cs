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


   public void ExecuteEffect(Action contentAction, Action endAction)
   {
      StartCoroutine(EffectRoutine(contentAction, endAction));
   }

   IEnumerator EffectRoutine(Action contentAction, Action endAction)
   {
      Stretch(true);
      yield return new WaitForSeconds(stretchDuration);
      
      contentAction?.Invoke();
      _rotationEffect.ExecuteRotation();
      
      yield return new WaitForSeconds(.2f);
      endAction?.Invoke();

      yield return new WaitForSeconds(rotationDuration-.2f);
      
      Stretch(false);
      _rotationEffect.ResetRotation();
      yield return new WaitForSeconds(stretchDuration);
      
      //endAction?.Invoke();
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
