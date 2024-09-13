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

        private Dictionary<int, TowerData> doubles = new();
        private Dictionary<int, TowerData> singles = new();
        public List<TowerData> SafeGroup { get; set; } = new();

        public void GetDoubles(List<int> newDoubleIds)
        {
            ResetDoubles();
            foreach (var id in newDoubleIds)
            {
                doubles.Add(id, AllTowers.GetData(id));
            }
        }
        
        public void GetTowers(int[] newTowers)
        {
            Towers = newTowers;

            ResetSingles();
            foreach (var id in newTowers)
            {
                if (!doubles.ContainsKey(id))
                {
                    singles.Add(id, AllTowers.GetData(id));
                }
            }
        }

        public List<int> SetDoublesClickable()
        {
            return doubles.Keys.Concat(singles.Keys).ToList();
        }
        

        void ResetDoubles()
        {
            doubles.Clear();
        }

        void ResetSingles()
        {
            singles.Clear();
        }
        
        public void TowerSelected(params object[] args)
        {
            int towerID = (int) args[0];
            
            if(doubles.ContainsKey(towerID))
            {
                DoubleRise(1);
            }
            
            else if (singles.ContainsKey(towerID))
            {
                SelectedSingleRise(singles[towerID], 1);
            }
        }

       

        void DoubleRise(int step)
        {
            int freeSingleResource = GetRiseHeightForDouble(step);
            
            int minDoubleResource = doubles.Count * step; //inebileceği resource
            if (freeSingleResource < minDoubleResource)
            {
                DoubleFall(step);
                return;
            }
            
          
            int rest = freeSingleResource % doubles.Count;

            if (rest > 0)
            {
                SafeGroup.RemoveAt(0); //todo: remove rest
                if(SafeGroup.Count == 0) return;
                freeSingleResource -= step; //todo: -= rest
            }
            
            int singleStep = freeSingleResource / doubles.Count;

            foreach (var tower in doubles.Values)
            {
                tower.Mover.ChangeHeight(tower.Height += singleStep, true);
            }
            
            SingleFall(1);

            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
          
        }

        void SingleFall(int step)
        {
            if (SafeGroup.Count < doubles.Count)
            {
                SafeGroup[0].Mover.ChangeHeight( SafeGroup[0].Height -= doubles.Count * step, false); //TODO tek singlea oynuyor yine
            }
            else
            {
                foreach (var safeSingle in SafeGroup)
                {
                    safeSingle.Mover.ChangeHeight(safeSingle.Height -= step, false);
                }
            }
        }

        int GetRiseHeightForDouble(int step)
        {
            SafeGroup.Clear();
           
            if (singles.Count < doubles.Count) //TODO: aslında 1 single'a göre çalışıyor bu şu an
            {
                int minimumRequiredStes = doubles.Count / singles.Count - 1;
                int surplus = doubles.Count - singles.Count;
                
                
                step *= doubles.Count;
                
                foreach (var tower in singles.Values) 
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
                foreach (var tower in singles.Values)
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

        bool DoubleFallCapacity(int step)
        {
            foreach (var tower in doubles.Values)
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
            foreach (var tower in doubles.Values)
            {
                tower.Mover.ChangeHeight(tower.Height -= step, false);
            }
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }
        
        void DoubleFall(int step)
        {
            if (!DoubleFallCapacity(step)) return;
            
            DoubleFallOperation(step);
            SerialSingleRise(step, doubles.Count * step);
        }

        void SerialSingleRise(int step, int freeDoubleResource)
        {
            if (singles.Count == doubles.Count)
            {
                foreach (var single in singles.Values)
                {
                    single.Mover.ChangeHeight(single.Height += step, true);
                }
            }
            else if (singles.Count > doubles.Count)
            {
                for (int i = 0; i < doubles.Count; i++)
                {
                    var tower = singles.ElementAt(i).Value;
                    tower.Mover.ChangeHeight(tower.Height += step, true);
                }
                
            }
            else //single az: 1 single fazladan yükselecek gibi
            {
                //singles.First().Value.Mover.ChangeHeight( singles.First().Value.Height += freeDoubleResource, true);
                
                 int loop = doubles.Count / singles.Count;
                 int rest = doubles.Count % singles.Count;
                //
                for (int i = 0; i < loop; i++)
                {
                    foreach (var tower in singles.Values)
                    {
                        tower.Mover.ChangeHeight(tower.Height += step, true); //changeheight üstüste çağrılabilir mi
                    }
                }
                
                for (int i = 0; i < rest; i++)
                {
                    var single = singles.ElementAt(i).Value;
                    single.Mover.ChangeHeight(single.Height += step, true); //todo singles i olamaz. i burda key gibi çalışır
                }
            }
        }
        
        void SelectedSingleRise(TowerData single, int step)
        {
            if (!DoubleFallCapacity(step))
            {
                DoubleRise(step);
                return;
            }
            
            DoubleFallOperation(step);
            
            int freeDoubleResource = doubles.Count * step;
            single.Mover.ChangeHeight(single.Height += freeDoubleResource, true);
        }
    }

}
