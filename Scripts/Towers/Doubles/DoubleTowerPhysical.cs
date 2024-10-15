using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Towers
{
    public class DoubleTowerPhysical
    {
        private int[] _towerIDs;
        private List<TowerData> _towers = new();
        private int _amount;
        public DoubleTowerPhysical(int[] towerIDs)
        {
            _towerIDs = towerIDs;
            
            foreach (var id in towerIDs)
            {
                var tower = AllTowers.GetData(id);
                _towers.Add(tower);
            }
            _amount = _towers.Count;
        }
        public void Equalize() //bridgeden önce olmalı
        {
            int totalHeight = 0;
            foreach (var tower in _towers)
            {
                totalHeight += tower.Height;
            }

            int averageHeight = totalHeight / _amount;
            int rest = averageHeight % _amount;
            
            foreach (var tower in _towers)
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
            Eventbus.TowerEvents.OnBridgeAttempt?.Invoke(_towerIDs);
        }
        
        public void Shake()
        {
            //TODO İMPLEMENT LATER
        }
    }

}
