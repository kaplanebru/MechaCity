using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCam
{
   private Vector3 center;

   private float _distance;
   private float _rotationOffset;
   
   public void SetOffsets(float distance, float rotationOffset)
   {
      _distance = distance;
      _rotationOffset = rotationOffset;
   }
   
  
   public (Vector3 position, Quaternion rotation) GetLinkPos(Transform cam, params Vector3[] centers)
   {
      CalculateCenter(centers);
      var dir = (center - cam.transform.position).normalized;
      
      CenterWithDistance(dir);
      return (center, RotationWithOffset(dir));
      
      //todo: niyeyse tam ortayı vermiyor, check et
   }

   void CalculateCenter(Vector3[] centers)
   {
      center = Vector3.zero;
      foreach (var item in centers)
      {
         center += item;
      }
      center /= centers.Length;
   }

   void CenterWithDistance(Vector3 dir)
   {
      center += dir * -_distance;
   }

   Quaternion RotationWithOffset(Vector3 dir)
   {
      var rot = Quaternion.LookRotation(dir);
      rot *= Quaternion.Euler(-_rotationOffset, 0, 0);
      return rot;
   }
   
}
