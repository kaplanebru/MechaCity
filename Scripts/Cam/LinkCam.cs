using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCam
{
   private Vector3 center;
   public Vector3 GetCenter(params Vector3[] centers)
   {
      center = Vector3.zero;
      foreach (var item in centers)
      {
         center += item;
      }

      center /= centers.Length;
      return center;

      //move.Invoke(center, Quaternion.identity);
   }
}
