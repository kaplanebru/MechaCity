using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
   public CardData Data;
   public TextMeshPro titleHolder;
   public TextMeshPro descriptionHolder;

   private void Start() //TODO: Initialize
   {
      Setup();
   }

   public void Setup()
   {
      SetTexts();
      SetColor();
   }

   void SetTexts()
   {
      titleHolder.text = Data.Title;
      descriptionHolder.text = Data.Description;
   }

   void SetColor()
   {
      GetComponentInChildren<MeshRenderer>().material.color = Data.color;

   }
}
