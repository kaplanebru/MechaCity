using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerUIHandler : MonoBehaviour
{
   public TextMeshPro heightText;

   private void OnEnable() //TODO: Tower scriptinden yönet
   {
      Eventbus.UIEvents.OnTowerHeightChange += ChangeHeightUI;
   }

   void CreateHealthIndicator()
   {
      
   }

   void ChangeHeightUI(float height, Tower tower) //DoTween
   {
      if(tower.gameObject != gameObject) return;
      
      int heightInt = Mathf.RoundToInt(height);
      heightText.text = heightInt.ToString();
   }

   private void OnDisable()
   {
      Eventbus.UIEvents.OnTowerHeightChange -= ChangeHeightUI;

   }
}
