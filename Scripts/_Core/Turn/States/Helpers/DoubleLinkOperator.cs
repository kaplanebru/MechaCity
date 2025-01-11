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
                _actors.Add(actorID, ActorDB.Registry[actorID]);
            }
        }

        private ActorData selectedActor;

        public void TowerSelected(params object[] args) //todo: eski gruplu hale getirip bak
        {
            uint actorID = (uint) args[0];
            selectedActor = _actors[actorID];

            SelectOperation(_actors[actorID]);
        }

        void SelectOperation(ActorData actor)
        {
            if (actor.Type == ActorType.Standard)
                SelectedSingleRise(1);
            else
                SelectedDoubleRise(1);

            UIEventbus.OnApplyPossibility?.Invoke(true); //todo: temp
        }


        void SelectedSingleRise(int step)
        {
            if (!CanRiseByOthers(step))
            {
                SelectedActorFall(step);
                return;
            }

            SafeGroup.SetRemovalSteps(step);
            OthersFall();

            var totalResource = SafeGroup.TowerCount * step;
            selectedActor.Towers[0].UpdateHeight(totalResource);

            //MediatorEventbus.ChainMotionEvents.OnMotion?.Invoke();
        }

        void SelectedDoubleRise(int step)
        {
            if (!CanRiseByOthers(step))
            {
                SelectedActorFall(step);
                return;
            }

            int freeSingleResource = GetOthersResourceForDouble(step); //todo check: sadece singlelar mı?
            int singleStep = freeSingleResource / selectedActor.TowerAmount;

            for (var i = 0; i < selectedActor.Towers.Length; i++)
            {
                selectedActor.Towers[i].UpdateHeight(singleStep); //todo: burdaki singlestep kaydedilebilir remove için
            }

            OthersFall();

            MediatorEventbus.ChainMotionEvents.OnMotion?.Invoke();
        }

        int totalAvailableHeight;
        private List<ActorData> tempGroup = new();

        bool CanRiseByOthers(int step)
        {
            totalAvailableHeight = 0;
            tempGroup.Clear();

            foreach (var actor in _actors.Values)
            {
                if (selectedActor == actor) continue;

                int availableHeight = actor.TryGetAvailableHeightByStep(step);
                if (availableHeight > 0)
                {
                    totalAvailableHeight += actor.TryGetAvailableHeightByStep(step);
                    tempGroup.Add(actor);
                }
            }

            if (totalAvailableHeight >= selectedActor.GetTowerAmountsPlusStep(step))
            {
                if (SurpassesMaxHeight(step))
                    return false;

                SafeGroup.Convert(tempGroup);
                SafeGroup.OrderByDescending();

                return true;
            }

            return false;
        }

        bool SurpassesMaxHeight(int step)
        {
            int maxTowerHeightInActor = 0;
            int possibleResource;
            if (selectedActor.Type == ActorType.Standard)
                possibleResource = tempGroup.Sum(a => a.TowerAmount) * step;

            else
                possibleResource = selectedActor.GetTowerAmountsPlusStep(step);

            var endTotalHeight = possibleResource + selectedActor.GetTotalHeight();
            maxTowerHeightInActor = endTotalHeight / selectedActor.GetTowerAmountsPlusStep(step) +
                                    endTotalHeight % selectedActor.GetTowerAmountsPlusStep(step);

            // Debug.Log(possibleResource + " " + selectedActor.GetTotalHeight());
            // Debug.Log(endTotalHeight + " "+ endTotalHeight / selectedActor.TowerAmount +" " +  endTotalHeight % selectedActor.TowerAmount);
            return maxTowerHeightInActor > AllTowers.MaxTowerHeight;
        }

        int GetOthersResourceForDouble(int step)
        {
            return SafeGroup.TowerCount < selectedActor.TowerAmount
                ? ResourceByLessPopulation(step)
                : ResourceByMorePopulation(step);
        }


        int ResourceByLessPopulation(int step) //1 stepten fazla azalacaklar, selected double'a yetişmek için
        {
            int doubleFreeResource = selectedActor.GetTowerAmountsPlusStep(step);

            int counter = doubleFreeResource;

            while (counter > 0)
            {
                foreach (var key in SafeGroup.StepsPerTower.Keys.ToList())
                {
                    SafeGroup.StepsPerTower[key]++;
                    counter--;
                }
            }

            return doubleFreeResource;
        }

        int ResourceByMorePopulation(int step)
        {
            foreach (var key in SafeGroup.StepsPerTower.Keys.ToList())
            {
                SafeGroup.StepsPerTower[key] = step;
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


        void OthersFall()
        {
            foreach (var safeTower in SafeGroup.StepsPerTower.Keys)
            {
                safeTower.UpdateHeight(-SafeGroup.GetStepsToRemove(safeTower));
            }
        }


        System.Random random = new System.Random();
        private uint randomKey;

        void SelectedActorFall(int step)
        {
            if (selectedActor.TryGetAvailableHeightByStep(step) == 0)
            {
                NoResourceUI();
                return;
            }

            do
            {
                randomKey = _actors.Keys.ElementAt(random.Next(_actors.Count));
            } while (randomKey == selectedActor.ID);

            selectedActor = _actors[randomKey];
            SelectOperation(selectedActor);
        }


        void NoResourceUI()
        {
            Debug.Log("No possible motion with this resource"); //TODO: UI
        }
    }
}