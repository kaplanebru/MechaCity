using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Towers;
using UnityEngine;

namespace Turn
{
    public class SafeGroup
    {
        public List<ActorData> Actors = new();
        public Dictionary<TowerData, int> StepsByTower = new();
        public int TowerCount => StepsByTower.Count;

        public void Add(ActorData actor, int stepToRemove)
        {
            Actors.Add(actor);

            foreach (var id in actor.TowerIDs)
            {
                var tower = AllTowers.GetData(id);
                StepsByTower.Add(tower, stepToRemove);
            }
        }

        public void RemoveActor(ActorData actor)
        {
            Actors.Remove(actor);

            foreach (var tower in actor.Towers)
            {
                StepsByTower.Remove(tower);
            }
        }

        public void OrderByDescending()
        {
            Actors = Actors.OrderByDescending(a => a.Towers[0].AvailableHeight).ToList();
            StepsByTower = StepsByTower.OrderByDescending(t => t.Key.AvailableHeight).ToDictionary(t => t.Key, t => t.Value);
        }

        public void SetRemovalStep(TowerData tower, int stepToRemove)
        {
            StepsByTower[tower] = stepToRemove;
        }

        public void Clear()
        {
            Actors.Clear();
            StepsByTower.Clear();
        }
    }
}