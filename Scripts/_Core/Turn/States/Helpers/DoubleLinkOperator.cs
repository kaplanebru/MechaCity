using System.Collections;
using System.Collections.Generic;
using GameUI;
using Towers;
using UnityEngine;

namespace Turn
{
    public class DoubleLinkOperator : LinkOperator
    {
        private List<TowerData> doubles;
        private TowerData single;
        public override void TowerSelected(params object[] args)
        {
            int towerID = (int) args[0];
            
            CheckIfDouble(towerID);
            
            // RiseAndFall(AllTowers.GetData(towerID), 1);
            // MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

        void CheckIfDouble(int id)
        {
            var tower = AllTowers.GetData(id);

            if (tower.IsDouble)
            {
                SetDoubles(tower);
            }
            else
            {
                single = tower;
            }
        }

        void SetDoubles(TowerData tower)
        {
            doubles.Add(tower);
            FindOtherHalf(tower);
        }

        void FindOtherHalf(TowerData firstHalf)
        {
            foreach (var id in firstHalf.NeighbourIDs)
            {
                var neighbor = AllTowers.GetData(id);
                if(!neighbor.IsDouble) continue;
                doubles.Add(neighbor);
            }
        }
    
    }

}
