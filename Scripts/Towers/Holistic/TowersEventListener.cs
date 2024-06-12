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
            GeneralEventbus.OnTowersCreated += GetTowers;
            
            UIEventbus.OnTowerHeightChange += UIHeightChangeRequest;
            UIEventbus.OnHealthChange += AdjustIcons;
            
        }
        void GetTowers()
        {
            towers = AllTowers.TowerDatas.ToList();
            foreach (var t in AllTowers.TowerDatas) //TODO event atılabilir
            {
                t.Mover.SetHeight(1);
            }
        }
        
        private void AdjustIcons(int health, int id)
        {
            GeneralEventbus.OnHealthIconChangeRequest?.Invoke(health, id);
        }

        private void UIHeightChangeRequest(float height, int id)
        {
            var tower = towers.FirstOrDefault(t=>t.UniqID == id);
            tower.UIHandler.ChangeHeightUI(height);
        }

        private void OnDisable()
        {
            GeneralEventbus.OnTowersCreated -= GetTowers;
            
            UIEventbus.OnTowerHeightChange -= UIHeightChangeRequest;
            UIEventbus.OnHealthChange -= AdjustIcons;
        }
    }

}
