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
        private List<TowerData> Towers = new();
        public int TowerCount => StepsByTower.Count;

        public int GetStepsToRemove(TowerData tower) => StepsByTower[tower];

        public void Add(ActorData actor)
        {
            Actors.Add(actor);

            foreach (var id in actor.TowerIDs)
            {
                var tower = AllTowers.GetData(id);
                StepsByTower.Add(tower, 0);
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
        
        

        public void SetRemovalSteps(int step)
        {
            foreach (var key in StepsByTower.Keys.ToList())
            {
                StepsByTower[key] = step;
            }
        }

        public void Clear()
        {
            Actors.Clear();
            StepsByTower.Clear();
        }
    }
}