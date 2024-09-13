using System.Collections.Generic;
using Towers;

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
        {
            
        }

        public bool InspectDoubleById(int id)
        {
            if (towers.ContainsKey(id))
                return true;
            return false;
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