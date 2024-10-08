using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Towers;
using UnityEngine;

namespace Turn
{
    public class DoubleLinkSetter 
    {
       
        private HashSet<DoubleTower> TurnDoubles = new();
        private Dictionary<int, TowerData> Singles = new();
        private int[] Towers;
        
        
        
        public void SetTowers(int[] newTowers, out HashSet<DoubleTower> turnDoubles, out Dictionary<int, TowerData> singles)
        {
            Towers = newTowers;
            
            TurnDoubles.Clear();
            Singles.Clear();
            
            SetSelectedTowers();
            
            turnDoubles = TurnDoubles;
            singles = Singles;
        }
        
        void SetSelectedTowers()
        {
            foreach (var id in Towers)
            {
                if (AllDoubles.InspectTower(id))
                {
                    TurnDoubles.Add(AllDoubles.GetDoubleByTower(id));
                }
                else
                {
                    Singles.Add(id, AllTowers.GetData(id));
                }
            }
        }

        public IEnumerable<int> SetTransferData()
        {
            return Singles.Keys.Concat(TurnDoubles.SelectMany(Double => Double.towers.Keys));
        }
    }

}
