using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Health;
using Towers;
using UnityEngine;

namespace Actor
{
    public class ActorDB
    {
        public static Dictionary<uint, ActorData> Registry { get; private set; } = new();
        public static Dictionary<Enums.ActorUnit, ActorUnit> Units = new();

        public static ActorData GetActor(uint id) => Registry[id];
        public static int[] GetTowerIDs(uint id) => Registry[id].TowerIDs;
        public static List<TowerNumericData> GetTowersData(uint id) => Registry[id].TowerNumericDatas.ToList();

        public static List<TowerData> GetTowerHeightCouples(uint id) =>
            Registry[id].Towers.ToList();


        private void Subscribe()
        {
            Eventbus.ActorEvents.OnDoubleTowerCreated += RegisterDouble;
        }

        public void Initialize()
        {
            SetControllers();
            Subscribe();
        }

        void SetControllers()
        {
            Units[Enums.ActorUnit.Health] = new HealthUnit(this);

            foreach (var unit in Units.Values)
            {
                unit.Subscribe();
            }
        }

        public void RegisterItem(ActorType type, int row, int health, params int[] ownTowers)
        {
            var id = UniqueIdGenerator.UIntId();
            var actor = new ActorData(id, type, ownTowers);
            actor.TeamType = AllTowers.GetNumericData(ownTowers[0]).TeamType; //todo: temporary

            Registry.Add(id, actor);
            actor.Row = row;
            ((HealthUnit) Units[Enums.ActorUnit.Health]).SetHealth(Registry[id], health, true);

            foreach (var towerID in ownTowers) //todo: register actor dataya eklenebilir
            {
                var tower = AllTowers.GetData(towerID);
                tower.VisualData.SetClickHandlerID(id);
            }
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

            OrderRegistryByRow();
            OnRegistryUpdate();
        }

        internal void OnRegistryUpdate()
        {
            Eventbus.ActorEvents.OnRegistryUpdate?.Invoke(Registry.Keys.ToArray());
        }

        internal void OrderRegistryByRow()
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
            foreach (var unit in Units.Values)
            {
                unit.Unsubscribe();
            }

            Eventbus.ActorEvents.OnDoubleTowerCreated -= RegisterDouble;

            Registry.Clear();
        }
    }
}