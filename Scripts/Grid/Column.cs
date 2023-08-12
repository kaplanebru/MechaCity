using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class Column
{
   public Slot[] Lines = new Slot[GameGrid.LineAmount];

   void CheckAvailableLines()
   {
      foreach (var line in Lines)
      {
         if (line.available) break;
      }
   }
}
