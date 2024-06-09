using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeSpawner : MonoBehaviour
{
   public Transform tubeObj;
   public List<Transform> towerTransforms;
   public List<Vector3> directions;

   private void OnEnable()
   {
      SetDirections();
      CreateTubes();
   }

   void SetDirections()
   {
      foreach (var towerTransform in towerTransforms)
      {
         var dir = (towerTransform.position - transform.position).normalized;
         directions.Add(new Vector3(dir.x, 0, dir.z).normalized);
      }
   }
   
   void CreateTubes()
   {
      foreach (var dir in directions)
      {
         var tube = Instantiate(tubeObj, transform);
         tube.transform.rotation = Quaternion.LookRotation(dir);
      }
   }

}
