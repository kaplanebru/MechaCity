using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Towers
{
    public class DTPhysical
    {
        private Dictionary<int, TowerData> _towers = new();
        private int _amount;
        public DTPhysical(Dictionary<int, TowerData> towers)
        {
            _towers = towers;
            _amount = towers.Count;
        }
        public void Equalize() //bridgeden önce olmalı
        {
            int totalHeight = 0;
            foreach (var tower in _towers.Values)
            {
                totalHeight += tower.Height;
            }

            int averageHeight = totalHeight / _amount;
            int rest = averageHeight % _amount;
            
            foreach (var tower in _towers.Values)
            {
                int extra = 0;
                if (rest > 0)
                {
                    extra = 1;
                    rest--;
                }
                var newHeight = averageHeight + extra;
                if(newHeight == tower.Height) continue;
                
                int surplus = newHeight - tower.Height;
                
                if(surplus==0)continue;
                tower.UpdateHeight(surplus);
                AllTowers.GetTower(tower.UniqID).StartRiseFallRoutine(true); //Todo: düzelt
            }
        }
        
        public void CreateBridge()
        {
            Eventbus.TowerEvents.OnBridgeAttempt?.Invoke(_towers.Keys.ToArray());
        }
        
        public void Shake()
        {
            //TODO İMPLEMENT LATER
        }
    }

}
