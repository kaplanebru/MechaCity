using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCam
{
   private Vector3 center;
   private LinkCamData Data;
   private Vector3 _worldCenter;
   
   public void Setup(LinkCamData data)
   {
      Data = data;
      _worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Data.WorldDistance));
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
      center += dir * -Data.Distance;
   }

   Quaternion RotationWithOffset(Vector3 dir)
   {
      var rot = Quaternion.LookRotation(dir);
      rot *= Quaternion.Euler(-Data.RotationOffset, 0, 0);
      return rot;
   }
   
}
