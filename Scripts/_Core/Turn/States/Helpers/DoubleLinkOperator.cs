using System.Collections;
using System.Collections.Generic;
using GameUI;
using Towers;
using UnityEngine;

namespace Turn
{
    public class DoubleLinkOperator : LinkOperator
    {
        private List<int> doublesId = new();
        private List<int> singlesId = new(); //todo: 3lü seçim de olabilir

        private List<TowerData> doubles = new();
        private List<TowerData> singles = new();

        //todo: selection olmadan da buluruz

        void Reset()
        {
            doublesId.Clear();
            singlesId.Clear();
            doubles.Clear();
            singles.Clear();
        }

        public void SetTowers(List<int> doubleTowers)
        {
            Reset();
            
            foreach (var id in towers)
            {
                if (doubleTowers.Contains(id))
                {
                    doublesId.Add(id);
                    doubles.Add(AllTowers.GetData(id));
                }
                else
                {
                    singlesId.Add(id);
                    singles.Add(AllTowers.GetData(id));
                }
            }
        }
        
        public override void TowerSelected(params object[] args)
        {
            int towerID = (int) args[0];

            if (doublesId.Contains(towerID))
            {
                RiseDouble(1);
            }
        }
        
        void RiseDouble(int step)
        {
            int totalStep = GetRiseHeightForDouble(step);

            if (totalStep <= step)
            {
                Fall(step);
                return;
            }
            
            int singleStep = totalStep / doublesId.Count;    //riseStep / 2 yaptığımızda tam sayı olmaz!!!
            int rest = totalStep % doublesId.Count;

            if (rest > 0)
            {
                safeGroup.RemoveAt(0);
                if(safeGroup.Count == 0) return;
            }

            foreach (var tower in doubles)
            {
                tower.Mover.ChangeHeight(tower.Height += singleStep, true);
            }
            foreach (var tower in safeGroup)
            {
                tower.Mover.ChangeHeight(tower.Height -= step, false);
            }
            
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
          
        }

        int GetRiseHeightForDouble(int step)
        {
            safeGroup.Clear();
            step *= doublesId.Count;
            
            foreach (var towerID in singlesId)
            {
                
                var tower = AllTowers.GetData(towerID);

                if (tower.AvailableHeight > step)
                {
                    safeGroup.Add(tower);
                }
            }
            
            return safeGroup.Count * step;
        }
        
        void Fall(int step)
        {
            int height = 0;
            foreach (var tower in doubles)
            {
                height += tower.height;
            }

            if (height > step * doubles.Count)
            {
                foreach (var tower in doubles)
                {
                    tower.Mover.ChangeHeight(tower.Height -= step, false);
                }

                ReverseFall(step);
               
                MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();

            }
            else
            {
                Debug.Log("Not enough resource to lift that tower!");
            }
        }

        void ReverseFall(int step)
        {
            if (singles.Count == doubles.Count)
            {
                foreach (var single in singles)
                {
                    single.Mover.ChangeHeight(single.Height += step, true);
                }
            }
            else if (singles.Count > doubles.Count)
            {
                for (int i = 0; i < doubles.Count; i++)
                {
                    singles[i].Mover.ChangeHeight(singles[i].Height += step, true);
                }
            }
            else
            {
                singles[0].Mover.ChangeHeight(singles[0].Height += step, true);
            }
        }
    }

}
