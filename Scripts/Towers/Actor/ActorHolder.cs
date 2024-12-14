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
        private Dictionary<Enums.ActorUnit, ActorUnit> units = new();

        public static ActorData GetActor(uint id) => Registry[id];
        public static int[] GetTowerIDs(uint id) => Registry[id].TowerIDs;
        public static List<TowerData> GetTowersData(uint id) => Registry[id].Towers.ToList();
        public void Subscribe()
        {
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady += FillRegistry;
            Eventbus.ActorEvents.OnDoubleTowerCreated += RegisterDouble;
        }

        public void Initialize()
        {
            SetControllers();
            Subscribe();
        }
        
        void SetControllers()
        {
            units[Enums.ActorUnit.Health] = new HealthUnit(this);

            foreach (var unit in units.Values)
            {
                unit.Subscribe();
            }
        }

        public void FillRegistry()
        {
            //todo linkler yapılabilir
            foreach (var tower in AllTowers.Towers)
            {
                var towerID = tower.Data.UniqID;
                RegisterItem(ActorType.Standard,towerID, tower.ConstantData.StartHealth, towerID);
            }
            OrderRegistry();
            OnRegistryUpdate();
        }

        public uint RegisterItem(ActorType type,int row, int health, params int[] ownTowers)
        {
            var id = UniqueIdGenerator.UIntId();
            var actor = new ActorData(id, type, ownTowers);
           
            Registry.Add(id, actor);
            actor.Row = row;
            ((HealthUnit)units[Enums.ActorUnit.Health]).SetHealth(Registry[id], health, true);
            
            foreach (var towerID in ownTowers)
            {
                var tower = AllTowers.GetData(towerID);
                tower.SetClickHandlerID(id);
            }
            return id;
        }

        private void RegisterDouble(uint[] oldActors)
        {
            int totalHealth = 0;
            List<int> ownTowers = new();
            int abortedRow = Registry[oldActors.First()].Row;
            
            foreach (var actorID in oldActors)
            {
                var actor = Registry[actorID];
                
                totalHealth += actor.Health;
                ownTowers.AddRange(actor.TowerIDs);
                RemoveItem(actor); //NOT: removelar'dan sonra register edildiği için doğru index'e geliyor, ama sona eklenip bug çıkarır sanıyordum.
            }
            
            RegisterItem(ActorType.MultiTower, abortedRow, totalHealth, ownTowers.ToArray());
            
            OrderRegistry();
            OnRegistryUpdate();
        }

        void OnRegistryUpdate()
        {
            Eventbus.ActorEvents.OnRegistryUpdate?.Invoke(Registry.Keys.ToArray());
        }

        void OrderRegistry()
        {
            Registry = Registry.OrderBy(a => a.Value.Row).ToDictionary(a => a.Key, a => a.Value);
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
        
        public static IEnumerable<uint> GetActiveActors(uint[] actorIDs)
        {
            foreach (var actorID in actorIDs)
            {
                var actor = Registry[actorID];
                if (!actor.ActivityStatus.CanMove) continue;
                yield return actorID;
            }
        }
        
        
        private void RemoveItem(ActorData actor)
        {
            Registry.Remove(actor.ID);
            Eventbus.HealthEvents.OnRemoveFromRegistry?.Invoke(actor.TowerIDs);
        }
        

        public void Unsubscribe()
        {
            foreach (var unit in units.Values)
            {
                unit.Unsubscribe();
            }
            Eventbus.ActorEvents.OnDoubleTowerCreated -= RegisterDouble;
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady -= FillRegistry;

            Registry.Clear();
        }
    }
}