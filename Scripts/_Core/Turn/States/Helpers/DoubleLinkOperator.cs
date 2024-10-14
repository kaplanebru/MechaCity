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
      
       
        private Dictionary<uint, ActorData> _actors = new();
        private SafeGroup SafeGroup = new();
        


        //Açıklama: normalde çoklu seçimde rise fall'a göre belirleniyor. diğer towerların fall'u ne kadarsa seçilen tower'a o kadar ekleniyor.
        //Fakat double towers amount > others olduğunda tam tersi çalışıyor: Others  souble tower height'ine ulaşana kadar 1'den fazla iner

        public void SetTowers(uint[] actors)
        {
            _actors.Clear();
            foreach (var actorID in actors)
            {
                _actors.Add(actorID, ActorHolder.Registry[actorID]);
            }
            
        }

        private ActorData selectedActor;

        public void TowerSelected(params object[] args) //todo: eski gruplu hale getirip bak
        {
            uint actorID = (uint) args[0];
            selectedActor = _actors[actorID];
           

            if (_actors[actorID].Type == ActorType.Standard)
            {
                //selection = AllTowers.GetData(_actors[actorID].TowerIDs[0]);
                SelectedSingleRise(1);
            }
            else
            {
               // selection = AllDoubles.GetDouble(actorID);
                SelectedDoubleRise(1);
            }

            UIEventbus.OnApplyPossibility?.Invoke(true); //todo: temp
        }


        void SelectedSingleRise(int step)
        {
            if (!CanDoubleRiseByOthers(step))
            {
                SelectedSingleFall(step);
                return;
            }

            OthersFall();

            var totalResource = SafeGroup.TowerCount * step;
            selectedActor.Towers[0].UpdateHeight(totalResource);
            
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

        void SelectedDoubleRise(int step)
        {
            if (!CanDoubleRiseByOthers(step))
            {
                SelectedDoubleFall( step);
                return;
            }
            
            //-----
            
            int freeSingleResource = GetOthersResourceForDouble(step);
            int singleStep = freeSingleResource / selectedActor.TowerAmount;

            foreach (var tower in selectedActor.Towers)
            {
                tower.UpdateHeight(singleStep); //todo: burdaki singlestep kaydedilebilir remove için
            }

            OthersFall();

            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

        int totalAvailableHeight = 0;
        bool CanDoubleRiseByOthers(int step)
        {
            totalAvailableHeight = 0;
            SafeGroup.Clear();

            foreach (var actor in _actors.Values)
            {
                if(selectedActor == actor) continue;

                int availableHeight = actor.TryGetAvailableHeight(step);
                if (availableHeight > 0)
                {
                    totalAvailableHeight += actor.TryGetAvailableHeight(step);
                    SafeGroup.Add(actor, step);
                }
            }
            SafeGroup.OrderByDescending();
            return totalAvailableHeight >= selectedActor.GetFreeResource(step);
        }
        

        int GetOthersResourceForDouble(int step)
        {
            return SafeGroup.TowerCount < selectedActor.TowerAmount
                ? ResourceByLessPopulation(step)
                : ResourceByMorePopulation(step);
        }


        int ResourceByLessPopulation(int step) //1 stepten fazla azalacaklar, selected double'a yetişmek için
        {
            int doubleFreeResource = selectedActor.GetFreeResource(step);
            int counter = doubleFreeResource;

            while (counter > 0)
            {
                foreach (var key in SafeGroup.StepsByTower.Keys.ToList())
                {
                    SafeGroup.StepsByTower[key]++;
                    counter--;
                }
            }
            
            return doubleFreeResource;
        }

        int ResourceByMorePopulation(int step)
        {
            foreach (var key in SafeGroup.StepsByTower.Keys.ToList())
            {
                SafeGroup.StepsByTower[key] = step;
            }

            CheckRest:
            var rest = SafeGroup.TowerCount % selectedActor.TowerAmount;
            if (rest > 0)
            {
                for (int i = 0; i < rest; i++)
                {
                    var actor = SafeGroup.Actors[i];
                    if (actor.Type == ActorType.MultiTower)
                    {
                        SafeGroup.RemoveActor(actor);
                        goto CheckRest;
                    }

                    SafeGroup.RemoveActor(actor);
                    //todo: safe groupta double varsa önce check et
                    //double olmayanlardan çıkar
                    //double varsa 2sini birden çıkar, kalana +1 fall amount ekle
                    //goto: check rest
                    //not: sondakiler muhtemelen doubledır, double en son ekleniyor
                }
            }
            
            return SafeGroup.TowerCount * step;
        }

        void OthersRise()
        {
            foreach (var tower in SafeGroup.StepsByTower.Keys)
            {
                tower.UpdateHeight(SafeGroup.StepsByTower[tower]);
            }
        }

        void OthersFall()
        {
            foreach (var tower in SafeGroup.StepsByTower.Keys)
            {
                tower.UpdateHeight(-SafeGroup.StepsByTower[tower]);
            }
        }

        void SelectedDoubleFall(int step)
        {
            if (selectedActor.TryGetAvailableHeight(step) == 0)
            {
                NoResourceUI();
                return;
            }

            foreach (var tower in selectedActor.Towers)
            {
                tower.UpdateHeight(-step); //potential bug: neden -SafeGroup.Towers.Count * step değil? Totaldekine uyumlu şekilde azalmalı
            }
           
            OthersRise();

            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }


        void SelectedSingleFall(int step)
        {
            if (selectedActor.Towers[0].AvailableHeight < step)
            {
                NoResourceUI();
                return;
            }

            OthersRise();
            selectedActor.Towers[0].UpdateHeight(-SafeGroup.TowerCount * step);

            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }

        void NoResourceUI()
        {
            Debug.Log("No possible motion with this resource"); //TODO: UI
        }
    }
}