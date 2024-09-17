using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Turn
{
    public class DoubleTower
    {
        public Dictionary<int, TowerData> towers = new();
        public int Amount;
        
        public DoubleTower(params int[] ids)
        {
            foreach (var id in ids)
            {
                towers.Add(id, AllTowers.GetData(id));
            }

            Amount = towers.Count;
        }

        public void GetDoubleById(int id)
        {}

        public bool InspectDoubleById(int id)
        {
            if (towers.ContainsKey(id))
                return true;
            return false;
        }
        
        public bool HasDoubleFallCapacity(int step)
        {
            foreach (var tower in towers.Values)
            {
                if (tower.height <= step)
                {
                    Debug.Log("not enough double resource for Fall");
                    return false;
                }
            }
            return true;
        }
        
        public void DoubleFallOperation(int step)
        {
            foreach (var tower in towers.Values)
            {
                tower.Mover.ChangeHeight(tower.Height -= step, false);
            }

            //MediatorEventbus.ChainMotionEvents.OnRising?.Invoke(); //TODO: 2 kez çağrılıyor olabilir
        }

        // public void AddDoubleById(int id) //todo: bool da yapılabilir
        // {
        //     if (!towers.ContainsKey(id))
        //     {
        //         towers.Add(id, AllTowers.GetData(id));
        //     }
        // }
    }
}