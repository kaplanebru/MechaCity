using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameUI;
using Towers;
using UnityEngine;

namespace Towers
{
    public class TowersEventListener : MonoBehaviour
    {
        public List<TowerData> towers = new();

        private void OnEnable()
        {
            GeneralEventbus.InitializerEvents.OnTowersCreated += GetTowers;
            
            UIEventbus.OnTowerHeightChange += UIHeightChangeRequest;
          
        }
        

        void GetTowers()
        {
            towers = AllTowers.TowerDatas.ToList();
        }
        
        private void AdjustDoubleIcons(int health, int id)
        {
            GeneralEventbus.OnAdjustDoubleIconsRequest?.Invoke(health, id);
        }
        
        
        
        private void UIHeightChangeRequest(float height, int id)
        {
            //print(id);
            var tower = towers.FirstOrDefault(t=>t.UniqID == id);
            tower?.UIHandler.ChangeHeightUI(height);
        }

        private void OnDisable()
        {
            GeneralEventbus.InitializerEvents.OnTowersCreated -= GetTowers;
            
            UIEventbus.OnTowerHeightChange -= UIHeightChangeRequest;
        }
    }

}
