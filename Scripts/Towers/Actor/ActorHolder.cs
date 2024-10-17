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
        private ActorEmployee[] Employees = new ActorEmployee[2];

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
            Employees[0] = new HealthEmployee(this);
            Employees[1] = new RelationEmployee(this);

            foreach (var controller in Employees)
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
                RegisterItem(ActorType.Standard,towerID, tower.ConstantData.StartHealth, towerID);
            }
            OrderRegistry();
        }

        public uint RegisterItem(ActorType type,int row, int health, params int[] ownTowers)
        {
            var id = UniqueIdGenerator.UIntId();
            var actor = new ActorData(id, type, ownTowers);
           
            Registry.Add(id, actor);
            actor.Row = row;
            ((HealthEmployee)Employees[0]).SetHealth(Registry[id], health, true);
            
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
            
            var id = RegisterItem(ActorType.MultiTower, abortedRow, totalHealth, ownTowers.ToArray());
            
            OrderRegistry();
            Eventbus.ActorEvents.OnDoubleTowerRegistered?.Invoke(); //Restore pairs
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

        // ActorData GetActorByTowerID(int towerID)
        // {
        //     var actorEntry = Registry.FirstOrDefault(a => a.Value.TowerIDs.Contains(towerID));
        //     return actorEntry.Value ?? null;
        // }
        
        private void RemoveItem(ActorData actor)
        {
            Registry.Remove(actor.ID);
            Eventbus.HealthEvents.OnRemoveFromRegistry?.Invoke(actor.TowerIDs);
        }
        

        public void Unsubscribe()
        {
            foreach (var controller in Employees)
            {
                controller.Unsubscribe();
            }
            Eventbus.ActorEvents.OnDoubleTowerCreated -= RegisterDouble;
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady -= FillRegistry;
            Registry.Clear();
        }
    }
}