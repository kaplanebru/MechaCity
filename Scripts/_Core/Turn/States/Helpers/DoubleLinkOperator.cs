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
        private List<TowerData> singles; //todo: 3lü seçim de olabilir
        
        //todo: selection olmadan da buluruz

        public void SetTowers()
        {
            foreach (var id in towers)
            {
                var tower = AllTowers.GetData(id);
                if (tower.IsDouble) //is Double yerine DoubleOperatörü çalıştıracak bir eventle de yollabilirler
                {
                    doubles.Add(tower);
                }
                else
                {
                    singles.Add(tower);
                }
            }
        }

        public void SetTowers2(List<int> doubleTowers)
        {
            doubles.Clear();
            singles.Clear();
            
            foreach (var id in towers)
            {
                var tower = AllTowers.GetData(id);
                if (doubleTowers.Contains(id))
                {
                    doubles.Add(tower);
                }
                else
                {
                    singles.Add(tower);
                }
            }
        }
        
        
        public override void TowerSelected(params object[] args)
        {
            int towerID = (int) args[0];
            
            

            // RiseAndFall(AllTowers.GetData(towerID), 1);
            // MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

       
    
    }

}
