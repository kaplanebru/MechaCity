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
            GeneralEventbus.IndicatorEvents.OnActorHover += SendTowersToIndicator;
        }

        private void SendTowersToIndicator(uint actorID)
        {
            var actor = Registry[actorID];
            //actor.Towers[0] //TODO: DOUBLE

            List<Vector3> othersPos = new();
            foreach (var linkedActor in actor.LinkedActors)
            {
                othersPos.Add(Registry[linkedActor].Center);
            }
            
            GeneralEventbus.IndicatorEvents.OnGettingIndicatorData?.Invoke(actor.Center,othersPos.ToArray());
            
        }

        public void Initialize()
        {
            SetControllers();
            Subscribe();
        }
        
        void SetControllers()
        {
            units[Enums.ActorUnit.Health] = new HealthUnit(this);
            units[Enums.ActorUnit.Relation] = new RelationUnit(this);

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
            ((RelationUnit)units[Enums.ActorUnit.Relation]).SetRelations(Registry.Keys.ToList());
            //Eventbus.ActorEvents.OnRegistryUpdate?.Invoke(); //Restore pairs
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
            foreach (var unit in units.Values)
            {
                unit.Unsubscribe();
            }
            Eventbus.ActorEvents.OnDoubleTowerCreated -= RegisterDouble;
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady -= FillRegistry;
            GeneralEventbus.IndicatorEvents.OnActorHover -= SendTowersToIndicator;

            Registry.Clear();
        }
    }
}