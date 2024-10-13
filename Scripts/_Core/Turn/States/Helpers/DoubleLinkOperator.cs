using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using GameUI;
using Towers;
using Unity.VisualScripting;
using UnityEngine;

namespace Turn
{
    public class DoubleLinkOperator : ILinkOperator
    {
        private LinkGroupMaker GroupMaker = new();
        private LinkGroupData GroupData = new();

        private Dictionary<TowerData, int> safeGroup = new();
        private ILinkable selection;

        private Dictionary<uint, ActorData> _actors = new();


        //Açıklama: normalde çoklu seçimde rise fall'a göre belirleniyor. diğer towerların fall'u ne kadarsa seçilen tower'a o kadar ekleniyor.
        //Fakat double towers amount > others olduğunda tam tersi çalışıyor: Others  souble tower height'ine ulaşana kadar 1'den fazla iner

        public void SetTowers(uint[] actors)
        {
            _actors.Clear();
            foreach (var actorID in actors)
            {
                _actors.Add(actorID, ActorHolder.Registry[actorID]);
            }

            GroupData = GroupMaker.GetGroups(actors);
        }

        public void TowerSelected(params object[] args)
        {
            uint actorID = (uint) args[0];
            
            if (_actors[actorID].Type == ActorType.Standard)
            {
                selection = AllTowers.GetData(_actors[actorID].Towers[0]);
                SelectedSingleRise(1);
            }
            else
            {
                selection = AllDoubles.GetDouble(actorID);
                SelectedDoubleRise(1);
            }
            
            UIEventbus.OnApplyPossibility?.Invoke(true); //todo: temp
        }

        void SelectedDoubleRise(int step)
        {
            DoubleTower selectedDouble = selection as DoubleTower;
            if (!CanDoubleRiseByOthers(step))
            {
                SelectedDoubleFall(selectedDouble, step);
                return;
            }

            int freeSingleResource = GetOthersResourceForDouble(step);
            int singleStep = freeSingleResource / selection.Amount;

            foreach (var tower in selectedDouble.towers.Values)
            {
                tower.UpdateHeight(singleStep);
            }

            OthersFall();

            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

        bool CanDoubleRiseByOthers(int step)
        {
            int totalAvailableHeight = 0;
            foreach (var tower in GroupData.Singles)
            {
                if (tower == selection) continue;
                if (tower.AvailableHeight < step) continue;
                totalAvailableHeight += tower.AvailableHeight;
            }

            foreach (var doubleTower in GroupData.TurnDoubles) //todo: check, new 2
            {
                if (doubleTower == selection) continue;
                if (doubleTower.NoDoubleFallCapacity(step)) continue;
                totalAvailableHeight += doubleTower.AvailableHeight;
            }
            
            return totalAvailableHeight >= selection.GetFreeResource(step);
        }

        int GetOthersResourceForDouble(int step)
        {
            CreateSafeGroup(step);

            return safeGroup.Count < selection.Amount
                ? ResourceByLessPopulation(step)
                : ResourceByMorePopulation(step);
        }

        void CreateSafeGroup(int step)
        {
            safeGroup.Clear();
            foreach (var tower in GroupData.Singles)
            {
                if (tower.AvailableHeight < step) continue;
                safeGroup.Add(tower, 0);
            }

            foreach (var doubleTower in GroupData.TurnDoubles) //todo: check, new
            {
                if (doubleTower == selection) continue;
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
            int doubleFreeResource = selection.GetFreeResource(step);
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

            CheckRest:
            var rest = safeGroup.Count % selection.Amount;
            if (rest > 0)
            {
                var safeTower = safeGroup.First().Key; //safeGroup.Last().Key

                for (int i = 0; i < rest; i++)
                {
                    foreach (var doubleTower in GroupData.TurnDoubles)
                    {
                        if (doubleTower.InspectByTowerData(safeTower))
                        {
                            foreach (var tower in doubleTower.towers)
                            {
                                safeGroup.Remove(tower.Value);
                            }
                            goto CheckRest;
                            //not: double hiç olmaya dabilir, o yüzden safe remove aşağıda
                        }
                        
                    }
                    safeGroup.Remove(safeTower);
                    
                    //todo: safe groupta double varsa önce check et
                    //double olmayanlardan çıkar
                    //double varsa 2sini birden çıkar, kalana +1 fall amount ekle
                    //goto: check rest
                    //not: sondakiler muhtemelen doubledır, double en son ekleniyor
                }
            }
            return safeGroup.Count * step;
        }

        void OthersRise()
        {
            foreach (var safePair in safeGroup)
            {
                var tower = safePair.Key;
                tower.UpdateHeight(safePair.Value);
            }
        }

        void OthersFall()
        {
            foreach (var safePair in safeGroup)
            {
                var tower = safePair.Key;
                tower.UpdateHeight(-safePair.Value);
            }
        }

        void SelectedDoubleFall(DoubleTower selectedDouble, int step)
        {
            if (selectedDouble.NoDoubleFallCapacity(step))
            {
                NoResourceUI();
                return;
            }

            selectedDouble.DoubleFallOperation(step);
            OthersRise();

            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

        void SelectedSingleRise(int step)
        {
            TowerData selectedSingle = selection as TowerData;
            if (!CanDoubleRiseByOthers(step))
            {
                SelectedSingleFall(selectedSingle, step);
                return;
            }
            
            CreateSafeGroup(step);
            safeGroup.Remove(selectedSingle);
            foreach (var key in safeGroup.Keys.ToList())
            {
                safeGroup[key] = step;
            }
            
            var totalResource = safeGroup.Count * step;
            OthersFall();

            selectedSingle.UpdateHeight(totalResource);
            
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

        void SelectedSingleFall(TowerData selectedSingle, int step)
        {
            if (selectedSingle.AvailableHeight < step)
            {
                NoResourceUI();
                return;
            }

            OthersRise();
            selectedSingle.UpdateHeight(-safeGroup.Count * step);
            
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

        void NoResourceUI()
        {
            Debug.Log("No possible motion with this resource"); //TODO: UI
        }
    }
}