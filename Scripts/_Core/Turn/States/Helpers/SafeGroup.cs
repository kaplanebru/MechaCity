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
        public Dictionary<TowerData, int> StepsPerTower = new();
        private List<TowerData> Towers = new();
        public int TowerCount => StepsPerTower.Count;

        public int GetStepsToRemove(TowerData tower) => StepsPerTower[tower];

        public void Add(ActorData actor)
        {
            Actors.Add(actor);

            foreach (var id in actor.TowerIDs)
            {
                var tower = AllTowers.GetData(id);
                StepsPerTower.Add(tower, 0);
            }
        }

        public void Convert(List<ActorData> actors)
        {
            Clear();
            foreach (var actor in actors)
            {
                Add(actor);
            }
        }

        public void RemoveActor(ActorData actor)
        {
            Actors.Remove(actor);

            foreach (var tower in actor.Towers)
            {
                StepsPerTower.Remove(tower);
            }
        }

        public void OrderByDescending()
        {
            Actors = Actors.OrderByDescending(a => a.Towers[0].AvailableHeight).ToList();
            StepsPerTower = StepsPerTower.OrderByDescending(t => t.Key.AvailableHeight).ToDictionary(t => t.Key, t => t.Value);
        }
        
        

        public void SetRemovalSteps(int step)
        {
            foreach (var key in StepsPerTower.Keys.ToList())
            {
                StepsPerTower[key] = step;
            }
        }

        public void Clear()
        {
            Actors.Clear();
            StepsPerTower.Clear();
        }
    }
}