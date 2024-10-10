using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameUI;
using UnityEngine;

namespace Health
{
    public class HealthUIEventListener : TowerRelatedEventListener<HealthHolder>
    {
        protected override HealthHolder[] RelatedItems { get; set; }
        public HealthHolder healthHolderPb;
        public override void Subscribe()
        {
            Eventbus.HealthEvents.OnHealthChange += AdjustHealthIcon;
            Eventbus.HealthEvents.OnRemoveFromRegistry += HideIcon;
            Eventbus.HealthEvents.OnDoubleHealthCreated += CreateCommonIcon;
        }
        
        public override void Initialize() { }
    
        private void AdjustHealthIcon(int health, int id)
        {
            var healthHolder = RelatedItems.FirstOrDefault(h => h.Id == id);
            healthHolder.AdjustIcons(health);
        }
        
        private void HideIcon(int id)
        {
            RelatedItems[id].icons.ForEach(i=>i.gameObject.SetActive(false));
        }

        public void CreateCommonIcon(int[] ids, int totalHealth, int doubleId)
        {
            HealthHolder[] holders = new HealthHolder[ids.Length];
            Vector3 center = Vector3.zero;
        
            for (var i = 0; i < ids.Length; i++)
            {
                holders[i] = RelatedItems.FirstOrDefault(h => h.Id == ids[i]);
                holders[i].DisableAll();
                center += holders[i].transform.position;
            }

            center /= holders.Length;
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
            Eventbus.HealthEvents.OnDoubleHealthCreated -= CreateCommonIcon;
        }
    }
}
