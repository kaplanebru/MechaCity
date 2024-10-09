using System.Collections.Generic;
using System.Linq;
using GameUI;
using Towers;
using UnityEngine;

namespace Towers
{
    public class DoubleTower: ILinkable
    {
        public Dictionary<int, TowerData> towers = new();
        
        public int Amount { get; set; }
        public int GetFreeResource(int step) =>  Amount * step;
        public int AvailableHeight //1-3'se mesela inemesin
        {
            get
            {
                return towers.Sum(tower => tower.Value.AvailableHeight);
            }
        }
        public bool InspectByTowerData(TowerData tower) => towers.ContainsValue(tower);
        public bool InspectByTowerID(int id) => towers.ContainsKey(id);
        
        public DoubleTower(params int[] ids)
        {
            foreach (var id in ids)
            {
                towers.Add(id, AllTowers.GetData(id));
            }
            
            towers = towers.OrderBy(t => t.Value.AvailableHeight).ToDictionary(t => t.Key, t => t.Value);
            Amount = towers.Count;
        }
        
        public bool NoDoubleFallCapacity(int step)
        {
            return towers.ElementAt(0).Value.AvailableHeight < step;
        }
        
        public void DoubleFallOperation(int step)
        {
            foreach (var tower in towers.Values)
            {
                tower.UpdateHeight(-step);
            }

            //MediatorEventbus.ChainMotionEvents.OnRising?.Invoke(); //TODO: 2 kez çağrılıyor olabilir
        }

        public bool Same(ILinkable other)
        {
            return other == this;
        }

        public void Equalize() //bridgeden önce olmalı
        {
            int totalHeight = 0;
            foreach (var tower in towers.Values)
            {
                totalHeight += tower.Height;
            }

            int averageHeight = totalHeight / Amount;
            int rest = averageHeight % Amount;
            
            foreach (var tower in towers.Values)
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
            
            CommonizeHealth();
        }
        
        public void CreateBridge()
        {
            Eventbus.TowerEvents.OnBridgeAttempt?.Invoke(towers.Keys.ToArray());
        }

        public void CommonizeHealth()
        {
            int totalHealth = towers.Sum(tower => tower.Value.Health);

            foreach (var tower in towers)
            {
                tower.Value.ChangeHealth(totalHealth);
            }
            UIEventbus.OnDoubleHealth?.Invoke(towers.Keys.ToArray());

        }
    }
}