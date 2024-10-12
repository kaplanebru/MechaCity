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
        public static Dictionary<string, ActorData> Registry { get; private set; } = new(); // TowerID -> Health
        public static int GetHealthByActor(string actorID) => Registry[actorID].Health;
        public static List<string> GetLinkedActors(string id) => Registry[id].LinkedActors;

        private ActorController[] Controllers = new ActorController[2];
        private HealthController HealthController;
        private RelationController RelationController;

        public void Subscribe()
        {
            Eventbus.ActorEvents.OnNewDoubleActor += RegisterDouble;
            SetControllers();
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

        public string RegisterItem(ActorType type, int initialHealth, params int[] ownTowers)
        {
            var id = UniqueIdGenerator.GenerateUniqueId();
            Registry.Add(id, new ActorData(id, type, initialHealth, ownTowers));
            return id;
        }

        public void RegisterDouble(params int[] ownTowers)
        {
            int totalHealth = 0;
            foreach (var tower in ownTowers)
            {
                var actor = GetActorByTowerID(tower);
                totalHealth += actor.Health;
                RemoveItem(actor.ID);
            }
            
            var id = RegisterItem(ActorType.MultiTower, totalHealth, ownTowers);
            //id = Registry.Last().Key;
            Eventbus.HealthEvents.OnHealthChange?.Invoke(id);
        }

        ActorData GetActorByTowerID(int towerID)
        {
            foreach (var actor in Registry)
            {
                if (actor.Value.Towers.Contains(towerID))
                    return actor.Value;
            }
            return null;
        }
        
        public void RemoveItem(string actorID)
        {
            Registry.Remove(actorID);
            Eventbus.HealthEvents.OnRemoveFromRegistry?.Invoke(actorID);
        }
        

        public void Unsubscribe()
        {
            Eventbus.ActorEvents.OnNewDoubleActor -= RegisterDouble;

            HealthController.Unsubscribe();
            RelationController.Unsubscribe();

            Registry.Clear();
        }
    }
}