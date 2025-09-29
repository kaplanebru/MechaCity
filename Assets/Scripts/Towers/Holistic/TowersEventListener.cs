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
            GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet += GetTowers;
            UIEventbus.OnTowerHeightChange += UIHeightChangeRequest;
        }
        

        void GetTowers()
        {
            towers = AllTowers.TowerDatas.Values.Select(t=>t).ToList();
        }
        
        private void UIHeightChangeRequest(int id)
        {
            var tower = towers.FirstOrDefault(t=>t.NumericData.UniqID == id);
            tower?.VisualData.UIHandler.ChangeHeightUI(tower.NumericData.Height); //height
        }

        private void OnDisable()
        {
            GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet -= GetTowers;
            UIEventbus.OnTowerHeightChange -= UIHeightChangeRequest;
        }
    }

}
