using System.Collections.Generic;
using System.Linq;
using Towers;
using UnityEngine;

namespace Turn
{
    public class DoubleTower
    {
        public Dictionary<int, TowerData> towers = new();
        public int Amount;
        
        public int GetFreeResource(int step) 
        {
            return Amount * step;
        }
        public int AvailableHeight //1-3'se mesela inemesin
        {
            get
            {
                return towers.Sum(tower => tower.Value.AvailableHeight);
            }
        }

        public DoubleTower(params int[] ids)
        {
            foreach (var id in ids)
            {
                towers.Add(id, AllTowers.GetData(id));
            }
            
            towers = towers.OrderBy(t => t.Value.AvailableHeight).ToDictionary(t => t.Key, t => t.Value);
            Amount = towers.Count;
        }
        
        public bool InspectDoubleById(int id)
        {
            if (towers.ContainsKey(id))
                return true;
            return false;
        }
        
        public bool NoDoubleFallCapacity(int step)
        {
            // if (towers.ElementAt(0).Value.AvailableHeight < step)
            //     return true;
            // return false;
           return towers.ElementAt(0).Value.AvailableHeight < step;
        }
        
        public void DoubleFallOperation(int step)
        {
            foreach (var tower in towers.Values)
            {
                tower.Mover.ChangeHeight(tower.Height -= step, false);
            }

            //MediatorEventbus.ChainMotionEvents.OnRising?.Invoke(); //TODO: 2 kez çağrılıyor olabilir
        }

    }
}