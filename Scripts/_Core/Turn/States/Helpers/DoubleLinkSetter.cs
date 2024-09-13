using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Towers;
using UnityEngine;

namespace Turn
{
    public class DoubleLinkSetter 
    {
        private List<DoubleTower> AllDoubles = new();
        private HashSet<DoubleTower> TurnDoubles = new();
        private Dictionary<int, TowerData> Singles = new();
        private int[] Towers;
        
        public void AddDoubles(DoubleTower newDouble)
        {
            AllDoubles.Add(newDouble);
        }

        public void RemoveDouble(DoubleTower doubleTower)
        {
            AllDoubles.Remove(doubleTower);
        }

        public bool InspectDoubles(int id)
        {
            foreach (var Double in AllDoubles)
            {
                if (Double.towers.ContainsKey(id))
                {
                    return true;
                }
            }
            return false;
        }
        
        public void SetTowers(int[] newTowers, out HashSet<DoubleTower> turnDoubles, out Dictionary<int, TowerData> singles)
        {
            Towers = newTowers;
            Debug.Log(Towers.Length);
            
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
                foreach (var Double in AllDoubles)
                {
                    if (Double.towers.ContainsKey(id))
                        TurnDoubles.Add(Double); //if(SD)Contains ise double gibi muamele edilir
                    
                    else
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
