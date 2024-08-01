using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class DoubleWithRival
    { 
        //Exchange Towerdaki selection çalışır
        
        public void HighlightNeighbours(int selectedTower)
        {
            var tower = AllTowers.GetData(selectedTower);
            foreach (var neighbourID in tower.NeighbourIDs)
            {
                AllTowers.GetData(neighbourID).ColorHandler.ToSelectionColor();
            }
            
            Debug.Log(tower.NeighbourIDs.Count);
        }
    }

}
