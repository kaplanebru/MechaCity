using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Health;
using Towers;
using UnityEngine;

namespace Actor
{
    public class ActorHolder
    {
        public static Dictionary<uint, ActorData> Registry { get; private set; } = new();
        private ActorController[] Controllers = new ActorController[2];

        public static ActorData GetActor(uint id) => Registry[id];

        public static int[] GetTowersByID(uint id) => Registry[id].TowerIDs;
        public void Subscribe()
        {
            SetControllers();
            Eventbus.ActorEvents.OnDoubleTowerCreated += RegisterDouble;
        }

        void SetControllers()
        {
            Controllers[0] = new HealthController(this);
            Controllers[1] = new RelationController(this);

            foreach (var controller in Controllers)
            {
                controller.Subscribe();
            }
        }

        public void FillRegistry()
        {
            //todo linkler yapılabilir
            foreach (var tower in AllTowers.Towers)
            {
                var towerID = tower.Data.UniqID;
                RegisterItem(ActorType.Standard, tower.ConstantData.StartHealth, towerID);
            }
        }

        public uint RegisterItem(ActorType type, int initialHealth, params int[] ownTowers)
        {
            var id = UniqueIdGenerator.UIntId();
            Registry.Add(id, new ActorData(id, type, initialHealth, ownTowers));
            
            foreach (var tower in ownTowers)              //TODO: LATER
            {
                AllTowers.GetData(tower).SetClickHandlerID(id);
            }
            return id;
        }

        private void RegisterDouble(int[] ownTowers)
        {
            int totalHealth = 0;
            foreach (var tower in ownTowers)
            {
                var actor = GetActorByTowerID(tower);
                totalHealth += actor.Health;
                RemoveItem(actor);
            }
            
            var id = RegisterItem(ActorType.MultiTower, totalHealth, ownTowers);
            
            Eventbus.HealthEvents.OnHealthChange?.Invoke(id);
            //return id;
        }

        public static List<int> ResolveTowersFromActors(uint[] actorIDs)
        {
            List<int> towers = new();
            foreach (var actorID in actorIDs)
            {
                foreach (var tower in Registry[actorID].TowerIDs)
                {
                    towers.Add(tower);
                }
            }

            return towers;
        }

        ActorData GetActorByTowerID(int towerID)
        {
            var actorEntry = Registry.FirstOrDefault(a => a.Value.TowerIDs.Contains(towerID));
            return actorEntry.Value ?? null;
        }
        
        private void RemoveItem(ActorData actor)
        {
            Registry.Remove(actor.ID);
            Eventbus.HealthEvents.OnRemoveFromRegistry?.Invoke(actor.TowerIDs);
        }
        

        public void Unsubscribe()
        {
            foreach (var controller in Controllers)
            {
                controller.Unsubscribe();
            }
            Eventbus.ActorEvents.OnDoubleTowerCreated -= RegisterDouble;

            Registry.Clear();
        }
    }
}