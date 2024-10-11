using System.Collections;
using System.Collections.Generic;
using Health;
using Towers;
using UnityEngine;

namespace Actor
{
    public class ActorManager
    {
        public static Dictionary<int, ActorData> Registry { get; private set; } = new(); // TowerID -> Health
        public static int GetHealth(int towerID) => Registry[towerID].Health;
        public static List<int> GetLinksByID(int id) => Registry[id].LinkedTowers;

        private ActorController[] Controllers = new ActorController[2];
        private HealthController HealthController;
        private RelationController RelationController;

        public void Subscribe()
        {
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
            foreach (var tower in AllTowers.Towers)
            {
                var id = tower.Data.UniqID;
                RegisterItem(id, tower.ConstantData.StartHealth);

            } //todo: double da register edilebilir
        }

        public void RegisterItem(int actorID, int initialHealth)
        {
            if (Registry.ContainsKey(actorID)) return;
            Registry[actorID] = new ActorData(initialHealth);
        }

        public void RemoveItem(int towerID)
        {
            Registry.Remove(towerID);
            Eventbus.HealthEvents.OnRemoveFromRegistry?.Invoke(towerID);
        }
        
     
        public void Unsubscribe()
        {
            HealthController.Unsubscribe();
            RelationController.Unsubscribe();

            Registry.Clear();
        }
    }
}