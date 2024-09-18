using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using GameUI;
using Towers;
using Unity.VisualScripting;
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
        private TowerData selectedSingle;


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
                selectedSingle = Singles[towerID];
                _selectedDouble = null;
                SelectedSingleRise(1);
            }
            else
            {
                foreach (var Double in TurnDoubles)
                {
                    if (!Double.InspectDoubleById(towerID)) continue;

                    selectedSingle = null;
                    _selectedDouble = Double;
                    SelectedDoubleRise(1);
                    break;
                }
            }

            UIEventbus.OnApplyPossibility?.Invoke(true); //todo: temp
        }

        void SelectedDoubleRise(int step)
        {
            if (!CanDoubleRiseByOthers(step))
            {
                SelectedDoubleFall(step);
                return;
            }

            int freeSingleResource = GetOthersResourceForDouble(step);
            int singleStep = freeSingleResource / _selectedDouble.Amount;

            foreach (var tower in _selectedDouble.towers.Values)
            {
                tower.Mover.ChangeHeight(tower.Height += singleStep, true);
            }

            OthersFall();

            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

        bool CanDoubleRiseByOthers(int step)
        {
            int totalAvailableHeight = 0;
            foreach (var tower in Singles.Values)
            {
                if (tower == selectedSingle) continue;
                if (tower.AvailableHeight < step) continue;
                totalAvailableHeight += tower.AvailableHeight;
            }

            foreach (var doubleTower in TurnDoubles) //todo: check, new 2
            {
                if (doubleTower == _selectedDouble) continue;
                if (doubleTower.NoDoubleFallCapacity(step)) continue;
                totalAvailableHeight += doubleTower.AvailableHeight;
            }

            return totalAvailableHeight >= _selectedDouble.GetFreeResource(step);
        }

        int GetOthersResourceForDouble(int step)
        {
            CreateSafeGroup(step);

            return safeGroup.Count < _selectedDouble.Amount
                ? ResourceByLessPopulation(step)
                : ResourceByMorePopulation(step);
        }

        void CreateSafeGroup(int step)
        {
            safeGroup.Clear();
            foreach (var tower in Singles.Values)
            {
                if (tower.AvailableHeight < step) continue;
                safeGroup.Add(tower, 0);
            }

            foreach (var doubleTower in TurnDoubles) //todo: check, new
            {
                if (doubleTower == _selectedDouble) continue;
                if (doubleTower.NoDoubleFallCapacity(step)) continue;

                foreach (var tower in doubleTower.towers)
                {
                    safeGroup.Add(tower.Value, 0);
                }
            }

            safeGroup = safeGroup.OrderByDescending(s => s.Key.AvailableHeight)
                .ToDictionary(s => s.Key, s => s.Value);
        }

        int ResourceByLessPopulation(int step) //1 stepten fazla azalacaklar, selected double'a yetişmek için
        {
            int doubleFreeResource = _selectedDouble.GetFreeResource(step);
            int counter = doubleFreeResource;

            while (counter > 0)
            {
                foreach (var key in safeGroup.Keys.ToList())
                {
                    safeGroup[key]++;
                    counter--;
                }
            }

            return doubleFreeResource;
        }

        int ResourceByMorePopulation(int step)
        {
            foreach (var key in safeGroup.Keys.ToList())
            {
                safeGroup[key] = step;
            }

            var rest = safeGroup.Count % _selectedDouble.Amount;
            if (rest > 0)
            {
                for (int i = 0; i < rest; i++)
                {
                    safeGroup.Remove(safeGroup.Last().Key); //todo: safe groupta double varsa işler değişir
                }
            }

            return safeGroup.Count * step;
        }

        void OthersRise()
        {
            //todo: en başta safe group oluşturmadan tıklanırsa bug olur
            foreach (var safePair in safeGroup)
            {
                var tower = safePair.Key;
                tower.Mover.ChangeHeight(tower.Height += safePair.Value, true);
            }
        }

        void OthersFall()
        {
            foreach (var safePair in safeGroup)
            {
                var tower = safePair.Key;
                tower.Mover.ChangeHeight(tower.Height -= safePair.Value, false);
            }
        }

        void SelectedDoubleFall(int step)
        {
            if (_selectedDouble.NoDoubleFallCapacity(step)) return;

            _selectedDouble.DoubleFallOperation(step);
            OthersRise();

            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

        void SelectedSingleRise(int step)
        {
            // if (safeGroup.Count == 0)
            // {
            //     SelectedSingleFall(selectedSingle, step);
            //     Debug.Log("no resource");
            //     return;
            // }
            if (CanDoubleRiseByOthers(1))
            {
                SelectedSingleFall(step);
                Debug.Log("no resource");
                return;
            }

            //_selectedDouble = null;
            CreateSafeGroup(step);
            safeGroup.Remove(selectedSingle);
            foreach (var key in safeGroup.Keys.ToList())
            {
                safeGroup[key] = step;
            }

            Debug.Log(safeGroup.Count);


            var totalResource = safeGroup.Count * step;
            OthersFall();

            selectedSingle.Mover.ChangeHeight(selectedSingle.Height += totalResource, true);
        }

        void SelectedSingleFall(int step)
        {
            if (selectedSingle.AvailableHeight < step) return;

            OthersRise();
            selectedSingle.Mover.ChangeHeight(selectedSingle.Height -= safeGroup.Count * step, true);
        }
    }
}