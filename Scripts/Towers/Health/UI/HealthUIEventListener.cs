using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using GameUI;
using UnityEngine;

namespace Health
{
    public class HealthUIEventListener: TowerRelatedEventListener<HealthHolder>
    {
        protected override HealthHolder[] RelatedItems { get; set; } //bunları hep dict yapmak lazım.
        public HealthHolder healthHolderPb;
        public override void Subscribe()
        {
            Eventbus.HealthEvents.OnHealthChange += AdjustHealthIcon;
            Eventbus.HealthEvents.OnRemoveFromRegistry += HideIcon;
          
        }
        
        public override void Initialize() { }
    
        private void AdjustHealthIcon(string actorID)
        {
            var actor = ActorHolder.Registry[actorID];

            if (actor.Type == ActorType.Standard)//(actor.Towers.Length == 1)
            {
                var towerID = actor.Towers.First();
                var healthHolder = RelatedItems.FirstOrDefault(h => h.Id == towerID);
                healthHolder.AdjustIcons(actor.Health);
                
            }
            else
            {
                CreateCommonIcon(actor.Towers.ToArray(), actor.Health);
            }
        }
        
        private void HideIcon(int[] towers)
        {
            foreach (var towerID in towers)
            {
                RelatedItems[towerID].icons.ForEach(i=>i.gameObject.SetActive(false));
            }
        }

        public void CreateCommonIcon(int[] towerIDs, int totalHealth)
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

            var health = Instantiate(healthHolderPb, holders[0].transform.parent);
            health.transform.position = center;
            health.AdjustIcons(totalHealth);
        
            //todo: iconlar diğer towerlardan ortaya dotweenle gelip toplanır, 10'a kadar çalışır
        }

        public override void Unsubscribe()
        {
            Eventbus.HealthEvents.OnHealthChange -= AdjustHealthIcon;
            Eventbus.HealthEvents.OnRemoveFromRegistry -= HideIcon;
        }
    }
}
