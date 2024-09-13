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
        public DoubleLinkSetter setter = new();
        
        private HashSet<DoubleTower> TurnDoubles = new();
        private Dictionary<int, TowerData> Singles = new();
        private DoubleTower _selectedDouble;
        public List<TowerData> SafeGroup { get; set; } = new();

        public void SetTowers(int[] newTowers)
        {
            setter.SetTowers(newTowers, out TurnDoubles, out Singles);
        }
        
        public void TowerSelected(params object[] args)
        {
            int towerID = (int) args[0];
            
            if (Singles.ContainsKey(towerID))
            {
                SelectedSingleRise(Singles[towerID], 1);
            }
            else
            {
                foreach (var Double in TurnDoubles)
                {
                    if(!Double.InspectDoubleById(towerID)) continue;
                    
                    _selectedDouble = Double;
                    DoubleRise(1);
                    break;
                }
            }
            UIEventbus.OnApplyPossibility?.Invoke(true); //todo: temp

        }
        
        void DoubleRise(int step)
        {
            int freeSingleResource = GetRiseHeightForDouble(step);
            
            int minDoubleResource = _selectedDouble.Amount * step; //inebileceği resource
            if (freeSingleResource < minDoubleResource)
            {
                DoubleFall(step);
                return;
            }

            int rest = freeSingleResource % _selectedDouble.Amount;

            if (rest > 0)
            {
                SafeGroup.RemoveAt(0); //todo: remove rest
                if(SafeGroup.Count == 0) return;
                freeSingleResource -= step; //todo: -= rest
            }
            
            int singleStep = freeSingleResource / _selectedDouble.Amount;

            foreach (var tower in _selectedDouble.towers.Values)
            {
                tower.Mover.ChangeHeight(tower.Height += singleStep, true);
            }
            
            SingleFall(1);

            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
          
        }

        void SingleFall(int step)
        {
            if (SafeGroup.Count < _selectedDouble.Amount)
            {
                SafeGroup[0].Mover.ChangeHeight( SafeGroup[0].Height -= _selectedDouble.Amount * step, false); //TODO tek singlea oynuyor yine
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
           
            if (Singles.Count < _selectedDouble.Amount) //TODO: aslında 1 single'a göre çalışıyor bu şu an
            {
                int minimumRequiredStes = _selectedDouble.Amount / Singles.Count - 1;
                int surplus = _selectedDouble.Amount - Singles.Count;
                
                
                step *= _selectedDouble.Amount;
                
                foreach (var tower in Singles.Values) 
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
                foreach (var tower in Singles.Values)
                {
                    if (tower.AvailableHeight > step)
                    {
                        counter++;
                        SafeGroup.Add(tower);
                        if(counter == _selectedDouble.Amount) break; 
                    }
                }
            }
            
            return SafeGroup.Count * step;
        }

        bool DoubleFallCapacity(int step)
        {
            foreach (var tower in _selectedDouble.towers.Values)
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
            foreach (var tower in _selectedDouble.towers.Values)
            {
                tower.Mover.ChangeHeight(tower.Height -= step, false);
            }
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }
        
        void DoubleFall(int step)
        {
            if (!DoubleFallCapacity(step)) return;
            
            DoubleFallOperation(step);
            SerialSingleRise(step, _selectedDouble.Amount * step);
        }

        void SerialSingleRise(int step, int freeDoubleResource)
        {
            if (Singles.Count == _selectedDouble.Amount)
            {
                foreach (var single in Singles.Values)
                {
                    single.Mover.ChangeHeight(single.Height += step, true);
                }
            }
            else if (Singles.Count > _selectedDouble.Amount)
            {
                for (int i = 0; i < _selectedDouble.Amount; i++)
                {
                    var tower = Singles.ElementAt(i).Value;
                    tower.Mover.ChangeHeight(tower.Height += step, true);
                }
                
            }
            else //single az: 1 single fazladan yükselecek gibi
            {
                //singles.First().Value.Mover.ChangeHeight( singles.First().Value.Height += freeDoubleResource, true);
                
                 int loop = _selectedDouble.Amount / Singles.Count;
                 int rest = _selectedDouble.Amount % Singles.Count;
                //
                for (int i = 0; i < loop; i++)
                {
                    foreach (var tower in Singles.Values)
                    {
                        tower.Mover.ChangeHeight(tower.Height += step, true); //changeheight üstüste çağrılabilir mi
                    }
                }
                
                for (int i = 0; i < rest; i++)
                {
                    var single = Singles.ElementAt(i).Value;
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
            
            int freeDoubleResource = _selectedDouble.Amount * step;
            single.Mover.ChangeHeight(single.Height += freeDoubleResource, true);
        }
    }

}
