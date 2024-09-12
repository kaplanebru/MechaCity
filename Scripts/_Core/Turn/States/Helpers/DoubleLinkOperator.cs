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

        private Dictionary<int, TowerData> doubleTowers = new();
        private Dictionary<int, TowerData> singleTowers = new();
        public List<TowerData> SafeGroup { get; set; } = new();
        

        public void GetDoubles(List<int> newDoubleIds)
        {
            ResetDoubles();
            foreach (var id in newDoubleIds)
            {
                doubleTowers.Add(id, AllTowers.GetData(id));
            }
        }
        
        public void GetTowers(int[] newTowers)
        {
            Towers = newTowers;

            ResetSingles();
            foreach (var id in newTowers)
            {
                if (!doubleTowers.ContainsKey(id))
                {
                    singleTowers.Add(id, AllTowers.GetData(id));
                }
            }
        }

        public List<int> SetDoublesClickable()
        {
            return doubleTowers.Keys.Concat(singleTowers.Keys).ToList();
        }
        

        void ResetDoubles()
        {
            doubleTowers.Clear();
        }

        void ResetSingles()
        {
            singleTowers.Clear();
        }
        
        public void TowerSelected(params object[] args)
        {
            int towerID = (int) args[0];
            
            if(doubleTowers.ContainsKey(towerID))
            {
                RiseDouble(1);
            }
            
            else if (singleTowers.ContainsKey(towerID))
            {
                RiseOneSingle(singleTowers[towerID], 1);
            }
        }

        void RiseOneSingle(TowerData single, int step)
        {
            if(!CheckDoubleFallPossibility(step)) return;
            DoubleFallOperation(step);
            
            int releasedResource = doubleTowers.Count * step;
            single.Mover.ChangeHeight(single.Height += releasedResource, true);
        }

        void RiseDouble(int step)
        {
            int totalStep = GetRiseHeightForDouble(step);

            if (totalStep < doubleTowers.Count * step)  //(totalStep <= step)
            {
                Fall(step);
                return;
            }
            
            int singleStep = totalStep / doubleTowers.Count;    //riseStep / 2 yaptığımızda tam sayı olmaz!!!
            int rest = totalStep % doubleTowers.Count;

            if (rest > 0)
            {
                SafeGroup.RemoveAt(0);
                if(SafeGroup.Count == 0) return;
            }

            foreach (var tower in doubleTowers.Values)
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
           
            if (singleTowers.Count < doubleTowers.Count)
            {
                step *= doubleTowers.Count;

                foreach (var tower in singleTowers.Values) //TODO: aslında 1 single'a göre çalışıyor bu şu an
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
                foreach (var tower in singleTowers.Values)
                {
                    if (tower.AvailableHeight > step)
                    {
                        counter++;
                        SafeGroup.Add(tower);
                        if(counter == doubleTowers.Count) break; 
                    }
                }
            }
            
            return SafeGroup.Count * step;
        }

        bool CheckDoubleFallPossibility(int step)
        {
            foreach (var tower in doubleTowers.Values)
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
            foreach (var tower in doubleTowers.Values)
            {
                tower.Mover.ChangeHeight(tower.Height -= step, false);
            }
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }
        void Fall(int step)
        {
            if(!CheckDoubleFallPossibility(step)) return;
            DoubleFallOperation(step);
            
            int releasedResource = doubleTowers.Count * step;
            ReverseFall(step, releasedResource);
        }

        void ReverseFall(int step, int releaedResource)
        {
            if (singleTowers.Count == doubleTowers.Count)
            {
                foreach (var single in singleTowers.Values)
                {
                    single.Mover.ChangeHeight(single.Height += step, true);
                }
            }
            else if (singleTowers.Count > doubleTowers.Count)
            {
                for (int i = 0; i < doubleTowers.Count; i++)
                {
                    singleTowers[i].Mover.ChangeHeight(singleTowers[i].Height += step, true);
                }
            }
            else
            {
                singleTowers[0].Mover.ChangeHeight(singleTowers[0].Height += releaedResource, true); //Todo maybe
            }
        }
    }

}
