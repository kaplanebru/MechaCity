using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using GameUI;
using Towers;
using UnityEngine;

namespace Turn
{
    public class DoubleLinkOperator : ILinkOperator
    {
        public LinkOperatorType Type { get; set; } = LinkOperatorType.Double;
        
        public int[] Towers { get; set; }
        
        public List<TowerData> SafeGroup { get; set; } = new();
        
        private List<int> doublesId = new();
        private List<int> singlesId = new(); //todo: 3lü seçim de olabilir

        private List<TowerData> doubles = new();
        private List<TowerData> singles = new();

        //todo: selection olmadan da buluruz
        
        public void GetTowers(int[] newTowers)
        {
            Towers = newTowers;

            foreach (var id in newTowers)
            {
                if (!doublesId.Contains(id))
                {
                    singlesId.Add(id);
                    singles.Add(AllTowers.GetData(id));
                }
            }
            
            SetDoublesClickable();
        }

        public void GetDoubles(List<int> newDoubles)
        {
            Reset();
            doublesId = newDoubles;
            foreach (var id in doublesId)
            {
                doubles.Add(AllTowers.GetData(id));
            }
        }

        public List<int> SetDoublesClickable()
        {
           return doublesId.Concat(singlesId).ToList();
        }
        

        void Reset()
        {
            doublesId.Clear();
            singlesId.Clear();
            doubles.Clear();
            singles.Clear();
        }
        
        public void TowerSelected(params object[] args)
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
                SafeGroup.RemoveAt(0);
                if(SafeGroup.Count == 0) return;
            }

            foreach (var tower in doubles)
            {
                tower.Mover.ChangeHeight(tower.Height += singleStep, true);
            }
            foreach (var tower in SafeGroup)
            {
                tower.Mover.ChangeHeight(tower.Height -= step, false);
            }
            
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
          
        }

        int GetRiseHeightForDouble(int step)
        {
            SafeGroup.Clear();
            step *= doublesId.Count;
            
            foreach (var towerID in singlesId)
            {
                
                var tower = AllTowers.GetData(towerID);

                if (tower.AvailableHeight > step)
                {
                    SafeGroup.Add(tower);
                }
            }
            
            return SafeGroup.Count * step;
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
