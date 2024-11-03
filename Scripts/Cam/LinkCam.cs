using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCam
{
   private Vector3 center;

   private float _distance;
   private float _rotationOffset;
   private float _worldDistance;
   private Vector3 _worldCenter;
   
   public void Setup(float distance, float rotationOffset, float worldDistance)
   {
      _distance = distance;
      _rotationOffset = rotationOffset;
      _worldDistance = worldDistance;
      _worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, _worldDistance));
   }
   
  
   public (Vector3 position, Quaternion rotation) GetLinkPos(Vector3[] centers)
   {
      CalculateCenter(centers);
      var dir = (center - _worldCenter).normalized;
      
      CenterWithDistance(dir);
      return (center, RotationWithOffset(dir));
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
