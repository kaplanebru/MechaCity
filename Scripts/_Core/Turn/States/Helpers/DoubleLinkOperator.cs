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
        private List<int> singlesId = new(); //todo: dict yapılabilir

        private List<TowerData> doubles = new();
        private List<TowerData> singles = new();

        public void GetDoubles(List<int> newDoubles)
        {
            ResetDoubles();
            doublesId = newDoubles;
            foreach (var id in doublesId)
            {
                doubles.Add(AllTowers.GetData(id));
            }
        }
        
        public void GetTowers(int[] newTowers)
        {
            Towers = newTowers;

            ResetSingles();
            foreach (var id in newTowers)
            {
                if (!doublesId.Contains(id))
                {
                    singlesId.Add(id);
                    singles.Add(AllTowers.GetData(id));
                }
            }
            
            //SetDoublesClickable();
        }

        public List<int> SetDoublesClickable()
        {
            // var clickables = doublesId.Concat(singlesId).ToList();
            // return clickables;
            return doublesId.Concat(singlesId).ToList();
        }
        

        void ResetDoubles()
        {
            doublesId.Clear();
            doubles.Clear();
        }

        void ResetSingles()
        {
            singlesId.Clear();
            singles.Clear();
        }
        
        public void TowerSelected(params object[] args)
        {
            int towerID = (int) args[0];

            if (doublesId.Contains(towerID))
            {
                RiseDouble(1);
            }
            else if (singlesId.Contains(towerID))
            {
                RiseOneSingle(AllTowers.GetData(towerID), 1);
            }
        }

        void RiseOneSingle(TowerData single, int step)
        {
            if(!CheckDoubleFallPossibility(step)) return;
            DoubleFallOperation(step);
            
            int releasedResource = doubles.Count * step;
            single.Mover.ChangeHeight(single.Height += releasedResource, true);
        }

        void RiseDouble(int step)
        {
            int totalStep = GetRiseHeightForDouble(step);

            if (totalStep < doubles.Count * step)  //(totalStep <= step)
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
           
            if (singles.Count < doubles.Count)
            {
                step *= doubles.Count;

                foreach (var tower in singles) //TODO: aslında 1 single'a göre çalışıyor bu şu an
                {
                    if (tower.AvailableHeight > step)
                    {
                        SafeGroup.Add(tower);
                    }
                }
            }
            else
            {
                int counter = 0;
                foreach (var tower in singles)
                {
                    if (tower.AvailableHeight > step)
                    {
                        counter++;
                        SafeGroup.Add(tower);
                        if(counter == doubles.Count) break; 
                    }
                }
            }
            
            return SafeGroup.Count * step;
        }

        bool CheckDoubleFallPossibility(int step)
        {
            foreach (var tower in doubles)
            {
                if (tower.height <= step)
                {
                    Debug.Log("not enough double resource for Fall");
                    return false;
                }
            }
            
            return true;
        }

        void DoubleFallOperation(int step)
        {
            foreach (var tower in doubles)
            {
                tower.Mover.ChangeHeight(tower.Height -= step, false);
            }
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }
        void Fall(int step)
        {
            if(!CheckDoubleFallPossibility(step)) return;
            DoubleFallOperation(step);
            
            int releasedResource = doubles.Count * step;
            ReverseFall(step, releasedResource);
            
            
            
            // int height = 0;
            // foreach (var tower in doubles)
            // {
            //     height += tower.height;
            // }
            //
            // if (height > step * doubles.Count)
            // {
            //     foreach (var tower in doubles)
            //     {
            //         tower.Mover.ChangeHeight(tower.Height -= step, false);
            //     }
            //
            //     ReverseFall(step);
            //    
            //     MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
            //
            // }
            // else
            // {
            //     Debug.Log("Not enough resource to lift that tower!");
            // }
        }

        void ReverseFall(int step, int releaedResource)
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
                singles[0].Mover.ChangeHeight(singles[0].Height += releaedResource, true); //Todo maybe
            }
        }
    }

}
