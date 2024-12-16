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
    public class HealthUIElementHolder : TowerRelatedElementHolder<HealthHolder>
    {
        private Dictionary<uint, HealthHolder> holdersByActor = new();

        protected override Dictionary<int, HealthHolder> RelatedItems { get; set; } = new();

        public override void Subscribe()
        {
            Eventbus.HealthEvents.OnHealthChange += AdjustHealthIcon;
            Eventbus.HealthEvents.OnRemoveFromRegistry += HideIcon;
        }

        public override void Initialize()
        {
        }

        private void AdjustHealthIcon(uint actorID)
        {
            var actor = ActorHolder.Registry[actorID];

            if (!holdersByActor.ContainsKey(actorID))
            {
                if (actor.Type == ActorType.Standard)
                {
                    var towerID = actor.TowerIDs.First();
                    //var healthHolder = RelatedItems.FirstOrDefault(h => h.Id == towerID);
                    var healthHolder = RelatedItems[towerID];
                    holdersByActor.Add(actorID, healthHolder);
                }
                else
                {
                    CreateCommonIcon(actorID, actor.TowerIDs, actor.Health);
                }
            }
            
            holdersByActor[actorID].AdjustIcons(actor.Health);
        }

        private void HideIcon(int[] towers)
        {
            foreach (var towerID in towers)
            {
                RelatedItems[towerID].icons.ForEach(i => i.gameObject.SetActive(false));
            }
        }
        
        void OnDoubleSeparated2(uint[] actorIDs)
        {
            ActorData[] actors = new ActorData[actorIDs.Length];
            HealthHolder[] holders = new HealthHolder[actorIDs.Length];
            for (var i = 0; i < actorIDs.Length; i++)
            {
                actors[i] = ActorHolder.Registry[actorIDs[i]];
            }

            foreach (var actorID in actorIDs)
            {
                var actor = ActorHolder.Registry[actorID];
                var holder = holdersByActor[actorID];
                
                var pos = holder.transform.position;
                pos.x = actor.Center.x;
                pos.z = actor.Center.z;
                holder.transform.position = pos;
                holder.gameObject.SetActive(true);
            }
        }

        void OnDoubleSeparated(uint actorID)
        {
            var actor = ActorHolder.Registry[actorID];
            var highestTower = actor.Towers.Aggregate((t1, t2) => t1.Height > t2.Height ? t1 : t2).UniqID;
            
            //var holder =  RelatedItems.FirstOrDefault(h => h.Id == highestTower);
            var holder = RelatedItems[highestTower];
            
             var pos = holder.transform.position;
             var tower = AllTowers.GetTower(highestTower);
             pos.x = tower.transform.position.x;
             pos.z = tower.transform.position.z;
             holder.transform.position = pos;
        }
        public void CreateCommonIcon(uint actorID, int[] towerIDs, int totalHealth)
        {
            HealthHolder[] holders = new HealthHolder[towerIDs.Length];
            Vector3 center = Vector3.zero;

            for (var i = 0; i < towerIDs.Length; i++)
            {
                //holders[i] = RelatedItems.FirstOrDefault(h => h.Id == towerIDs[i]);
                holders[i] = RelatedItems[towerIDs[i]];
                holders[i].DisableAll();
                center += holders[i].transform.position;
            }

            center /= holders.Length;
            holders = holders.OrderByDescending(t => t.transform.position.y).ToArray();
            center.y = holders[0].transform.position.y;

            holders[0].transform.position = center;
            holdersByActor.Add(actorID, holders[0]);
            

       

            //todo: iconlar diğer towerlardan ortaya dotweenle gelip toplanır, 10'a kadar çalışır
        }

        public override void Unsubscribe()
        {
            Eventbus.HealthEvents.OnHealthChange -= AdjustHealthIcon;
            Eventbus.HealthEvents.OnRemoveFromRegistry -= HideIcon;
        }
    }
}