using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using Towers;
using Unity.VisualScripting;
using UnityEngine;

namespace Turn
{
    public class DoubleLinkSetter 
    {
       
        private HashSet<DoubleTower> TurnDoubles = new();
        private Dictionary<int, TowerData> Singles = new();
        private int[] _towers;
        private uint[] _actors;
        
        
        
        public void SetTowers(uint[] actors, out HashSet<DoubleTower> turnDoubles, out Dictionary<int, TowerData> singles)
        {
            _actors = actors;
            
            
            _towers = newTowers;
            
            TurnDoubles.Clear();
            Singles.Clear();
            
            SetSelectedTowers();
            
            turnDoubles = TurnDoubles;
            singles = Singles;
        }
        
        void SetSelectedTowers()
        {
            foreach (var actorID in _actors)
            {
                var actor = ActorHolder.Registry[actorID];
                if (actor.Type == ActorType.MultiTower)
                {
                    TurnDoubles.AddRange(actor.Towers)
                }
            }
            
            
            foreach (var id in _towers)
            {
                if (AllDoubles.TryInspectTowerAndGetDouble(id, out DoubleTower doubleTower))
                {
                    TurnDoubles.Add(doubleTower);
                }
                else
                {
                    Singles.Add(id, AllTowers.GetData(id));
                }
            }
        }

        public IEnumerable<int> SetTransferData()
        {
            return Singles.Keys.Concat(TurnDoubles.SelectMany(Double => Double.towers.Keys));
        }
    }

}
