using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using UnityEngine;

public class Card : MonoBehaviour
{
   public CardData Data;

   private void Start() //TODO: Initialize
   {
      Setup();
   }

   public void Setup()
   {
      GetComponentInChildren<MeshRenderer>().material.color = Data.color;
   }
}
