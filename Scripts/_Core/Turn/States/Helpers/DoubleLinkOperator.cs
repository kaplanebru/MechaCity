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
        //Not: normalde çoklu seçimde rise fall'a göre belirleniyor. diğer towerların fall'u ne kadarsa seçilen tower'a o kadar ekleniyor.
        //Fakat double towers amount > others olduğunda tam tersi çalışıyor: Others  souble tower height'ine ulaşana kadar 1'den fazla iner

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
            int totalSingleAvailableHeight = 0;
            foreach (var tower in Singles.Values)
            {
                if (tower.AvailableHeight < step) continue;
                totalSingleAvailableHeight += tower.AvailableHeight;
                Debug.Log(" av height: " + tower.AvailableHeight);

            }
            
            return totalSingleAvailableHeight >= _selectedDouble.GetFreeResource(step);
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

            if (safeGroup.Count < _selectedDouble.Amount) //1 stepten fazla azalacaklar, selected double'a yetişmek için.
            {
                int counter = _selectedDouble.GetFreeResource(step);
                while (counter > 0)
                {
                    foreach (var key in keys)
                    {
                        safeGroup[key]++;
                        counter--;
                    }
                }
                return _selectedDouble.GetFreeResource(step);
            }
            

            foreach (var key in keys)
            {
                safeGroup[key] += step;
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
        
        void SelectedSingleRise(TowerData selectedSingle, int step)
        {
            int totalResource; 
            int freeDoubleResource = 0;
            int freeSingleResource;

            foreach (var doubleTower in TurnDoubles)
            {
                if (doubleTower.HasDoubleFallCapacity(step))
                {
                    freeDoubleResource += doubleTower.GetFreeResource(step);
                    doubleTower.DoubleFallOperation(step);
                }
            }
            
            safeGroup.Clear();
            foreach (var singleTower in Singles)
            {
                if(singleTower.Value == selectedSingle) continue;
                if (singleTower.Value.AvailableHeight >= step)
                {
                    safeGroup.Add(singleTower.Value, step);
                }
            }
            freeSingleResource = safeGroup.Count * step;

            totalResource = freeSingleResource + freeDoubleResource;
            if (totalResource < step)
            {
                // SelectedSingleFall(selectedSingle, step); //todo: kendi fall olacak -- kendinin fall olabilirliği kadar diğerleri rise olabilir
                
                Debug.Log("not enough Total Resource");
                return;
            }
            
            SingleFall();
            selectedSingle.Mover.ChangeHeight(selectedSingle.Height += totalResource, true);
        }

        void SelectedSingleFall(TowerData selectedSingle, int step)
        {
            //doublelearın fall'u ikili
        }
    }
}