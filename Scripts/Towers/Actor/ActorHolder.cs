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
                var actorID = RegisterItem(ActorType.Standard, towerID);
                ((HealthEmployee)Employees[0]).SetHealth(Registry[actorID], tower.ConstantData.StartHealth, true);
            }
        }

        public uint RegisterItem(ActorType type, params int[] ownTowers)
        {
            var id = UniqueIdGenerator.UIntId();
            var actor = new ActorData(id, type, ownTowers);
            Registry.Add(id, actor);
            //Registry.Insert()
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
            int index =  Registry.Keys.ToList().IndexOf(oldActors.First());
            
            foreach (var actorID in oldActors)
            {
                var actor = Registry[actorID];
                
                totalHealth += actor.Health;
                ownTowers.AddRange(actor.TowerIDs);
                RemoveItem(actor); //NOT: removelar'dan sonra register edildiği için doğru index'e geliyor, ama sona eklenip bug çıkarır sanıyordum.
            }
            
            var id = RegisterItem(ActorType.MultiTower, ownTowers.ToArray());
            
            ((HealthEmployee)Employees[0]).SetHealth(Registry[id], totalHealth, true);
            Eventbus.ActorEvents.OnDoubleTowerRegistered?.Invoke(); //Restore pairs
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