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

        public static int[] GetTowersByID(uint id) => Registry[id].Towers;
        public void Subscribe()
        {
            AllDoubles.DoubleTowerEvents.OnDoubleTowerCreated += RegisterDouble;
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

        private uint RegisterDouble(int[] ownTowers)
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
            return id;
        }

        public static List<int> ResolveTowersFromActors(uint[] actorIDs)
        {
            List<int> towers = new();
            foreach (var actorID in actorIDs)
            {
                towers.AddRange(GetTowersByID(actorID));
            }

            return towers;
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
        
        private void RemoveItem(ActorData actor)
        {
            Registry.Remove(actor.ID);
            Eventbus.HealthEvents.OnRemoveFromRegistry?.Invoke( actor.Towers);
        }
        

        public void Unsubscribe()
        {
            AllDoubles.DoubleTowerEvents.OnDoubleTowerCreated -= RegisterDouble;

            foreach (var controller in Controllers)
            {
                controller.Unsubscribe();
            }

            Registry.Clear();
        }
    }
}