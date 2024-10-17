using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using GameUI;
using Towers;
using UnityEngine;

namespace Health
{
    public class HealthUIEventListener : TowerRelatedEventListener<HealthHolder>
    {
        protected override HealthHolder[] RelatedItems { get; set; } //bunları hep dict yapmak lazım.
        private Dictionary<uint, HealthHolder> holdersByActor = new();
        public HealthHolder healthHolderPb;

        public override void Subscribe()
        {
            Eventbus.HealthEvents.OnHealthChange += AdjustHealthIcon;
            Eventbus.HealthEvents.OnRemoveFromRegistry += HideIcon;
        }

        public override void Initialize()
        {
        }

        // void SetHolderByActor()
        // {
        //     for (int i = 0; i < RelatedItems.Length; i++) //herkesin tekli başladığı senaryoda
        //     {
        //         holdersByActor.Add(ActorHolder.Registry.Keys.ElementAt(i), RelatedItems[i]);
        //     }
        // }

        private void AdjustHealthIcon(uint actorID)
        {
            var actor = ActorHolder.Registry[actorID];

            if (!holdersByActor.ContainsKey(actorID))
            {
                if (actor.Type == ActorType.Standard)
                {
                    var towerID = actor.TowerIDs.First();
                    var healthHolder = RelatedItems.FirstOrDefault(h => h.Id == towerID);
                    holdersByActor.Add(actorID, healthHolder);
                }
                else
                {
                    CreateCommonIcon(actorID, actor.TowerIDs, actor.Health);
                }
            }

            Debug.Log(actor.Health);
            holdersByActor[actorID].AdjustIcons(actor.Health);

         
        }

        private void HideIcon(int[] towers)
        {
            foreach (var towerID in towers)
            {
                RelatedItems[towerID].icons.ForEach(i => i.gameObject.SetActive(false));
            }
        }

        void OnDoubleSeparated(uint actorID)
        {
            var actor = ActorHolder.Registry[actorID];
            var highestTower = actor.Towers.Aggregate((t1, t2) => t1.Height > t2.Height ? t1 : t2).UniqID;
            
            var holder =  RelatedItems.FirstOrDefault(h => h.Id == highestTower);
            
             var pos = holder.transform.position;
             var tower = AllTowers.GetTower(highestTower);
             pos.x = tower.transform.position.x;
             pos.z = tower.transform.position.z;
        }

        // public void CreateCommon(ActorData actor, int totalHealth)
        // {
        //     foreach (var tower in actor.Towers)
        //     {
        //        // tower. //önce bütün hepsini kapat
        //     }
        //     
        //     //sonra en yüksek holderı aç. sorun en yüksek holderın mevcut healthini sıfırlamamaktan kaynaklı. Poolluk bişey yok.
        //     var health = Instantiate(healthHolderPb, actor.HealthParent);
        //     health.transform.position = actor.Center;
        //     health.AdjustIcons(totalHealth);
        // }

        public void CreateCommonIcon(uint actorID, int[] towerIDs, int totalHealth)
        {
            HealthHolder[] holders = new HealthHolder[towerIDs.Length];
            Vector3 center = Vector3.zero;

            for (var i = 0; i < towerIDs.Length; i++)
            {
                holders[i] = RelatedItems.FirstOrDefault(h => h.Id == towerIDs[i]);
                holders[i].DisableAll();
                center += holders[i].transform.position;
            }

            center /= holders.Length;
            holders = holders.OrderByDescending(t => t.transform.position.y).ToArray();
            center.y = holders[0].transform.position.y;

            holders[0].transform.position = center;
            holdersByActor.Add(actorID, holders[0]);
            

            //var health = Instantiate(healthHolderPb, holders[0].transform.parent);
            // health.transform.position = center;
            // health.AdjustIcons(totalHealth);

            //todo: iconlar diğer towerlardan ortaya dotweenle gelip toplanır, 10'a kadar çalışır
        }

        public override void Unsubscribe()
        {
            Eventbus.HealthEvents.OnHealthChange -= AdjustHealthIcon;
            Eventbus.HealthEvents.OnRemoveFromRegistry -= HideIcon;
        }
    }
}