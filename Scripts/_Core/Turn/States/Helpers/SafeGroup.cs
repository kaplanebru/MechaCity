using System.Collections;
using System.Collections.Generic;
using Actor;
using Towers;
using UnityEngine;

namespace Turn
{
    public class SafeGroup
    {
        public List<ActorData> Actors = new();
        public List<TowerData> Towers = new();
        public Dictionary<int, int> RemovalStepsByID = new();

        public void Add(ActorData actor)
        {
            Actors.Add(actor);

            foreach (var tower in actor.TowerIDs)
            {
                Towers.Add(AllTowers.GetData(tower));
                RemovalStepsByID.Add(tower, 0);
            }
            
            // safeGroup = safeGroup.OrderByDescending(s => s.Key.AvailableHeight)
            //     .ToDictionary(s => s.Key, s => s.Value);
        }

        public void RemoveItem(ActorData item, params int[] towerIDs)
        {
            Actors.Remove(item);

            foreach (var towerID in towerIDs)
            {
                var tower = AllTowers.GetData(towerID);
                Towers.Remove(tower);
                RemovalStepsByID.Remove(towerID);
            }
        }

        public void SetRemovalStep(int id, int stepToRemove)
        {
            RemovalStepsByID[id] = stepToRemove;
        }

        public void Clear()
        {
            Actors.Clear();
            Towers.Clear();
            RemovalStepsByID.Clear();
        }
    }
}