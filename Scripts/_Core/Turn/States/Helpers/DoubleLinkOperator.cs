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
        public DoubleLinkSetter setter = new();

        private HashSet<DoubleTower> TurnDoubles = new();
        private Dictionary<int, TowerData> Singles = new();

        private DoubleTower _selectedDouble;
        private Dictionary<TowerData, int> safeGroup = new();

        //TODO: ne fall ne rise söz konusu olmayan durum için sonsuz döngüyü engelle

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
                    if (!Double.InspectDoubleById(towerID)) continue;

                    _selectedDouble = Double;
                    DoubleRise(1);
                    break;
                }
            }
            UIEventbus.OnApplyPossibility?.Invoke(true); //todo: temp
        }


        bool HasDoubleRiseCapacity(int step)
        {
            int totalAvailableHeight = 0;
            foreach (var tower in Singles.Values)
            {
                if (tower.AvailableHeight < step) continue;
                totalAvailableHeight += tower.AvailableHeight;
                Debug.Log(" av height: " + tower.AvailableHeight);

            }

          
            return totalAvailableHeight >= _selectedDouble.Amount * step;
        }
        
        int GetRiseHeightForDouble(int step)
        {
            safeGroup.Clear();
            foreach (var tower in Singles.Values)
            {
                if (tower.AvailableHeight < step) continue;

                safeGroup.Add(tower, 0);
            }
            
            safeGroup = safeGroup.OrderByDescending(s => s.Key.AvailableHeight)
                .ToDictionary(s => s.Key, s => s.Value);
            var keys = safeGroup.Keys.ToList();

            if (safeGroup.Count < _selectedDouble.Amount)
            {
                int counter = _selectedDouble.Amount * step;
                while (counter > 0)
                {
                    foreach (var key in keys)
                    {
                        safeGroup[key]++;
                        counter--;
                    }
                }
                return _selectedDouble.Amount * step;
            }

            foreach (var key in keys)
            {
                safeGroup[key] += step; //bütün available height şart mı? 
            }

            var rest = safeGroup.Count % _selectedDouble.Amount;
            if (rest > 0)
            {
                for (int i = 0; i < rest; i++)
                {
                    safeGroup.Remove(safeGroup.Last().Key);
                }
            }
            return safeGroup.Count * step;
        }

        void DoubleRise(int step)
        {
            if (!HasDoubleRiseCapacity(step))
            {
                DoubleFall(step);
                return;
            }
            
            int freeSingleResource = GetRiseHeightForDouble(step);
            int singleStep = freeSingleResource / _selectedDouble.Amount;

            foreach (var tower in _selectedDouble.towers.Values)
            {
                tower.Mover.ChangeHeight(tower.Height += singleStep, true);
            }

            SingleFall();

            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

       

        void DoubleFall(int step)
        {
            if (!_selectedDouble.HasDoubleFallCapacity(step)) return;

            _selectedDouble.DoubleFallOperation(step);
            SingleRise();
            
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke(); //Sonradan eklendi
        }
        
        void SingleRise()
        {
            foreach (var safePair in safeGroup)
            {
                var tower = safePair.Key;
                tower.Mover.ChangeHeight(tower.Height += safePair.Value, true);
            }
        }

        void SingleFall()
        {
            foreach (var safePair in safeGroup)
            {
                var tower = safePair.Key;
                tower.Mover.ChangeHeight(tower.Height -= safePair.Value, false);
            }
        }
        
        void SelectedSingleRise(TowerData single, int step)
        {
            int totalResource; 
            int freeDoubleResource = 0;
            int freeSingleResource;

            foreach (var doubleTower in TurnDoubles)
            {
                if (doubleTower.HasDoubleFallCapacity(step))
                {
                    freeDoubleResource += doubleTower.Amount * step;
                    doubleTower.DoubleFallOperation(step);
                }
            }
            
            safeGroup.Clear();
            foreach (var singleTower in Singles)
            {
                if(singleTower.Value == single) continue;
                if (singleTower.Value.AvailableHeight >= step)
                {
                    safeGroup.Add(singleTower.Value, step);
                }
            }
            freeSingleResource = safeGroup.Count * step;

            totalResource = freeSingleResource + freeDoubleResource;
            if (totalResource < step)
            {
                //DoubleRise(step);
                Debug.Log("not enough Total Resource");
                return;
            }
            
            SingleFall();
            single.Mover.ChangeHeight(single.Height += totalResource, true);
        }
    }
}