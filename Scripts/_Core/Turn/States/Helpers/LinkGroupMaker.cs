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
    public class LinkGroupData
    {
        public HashSet<DoubleTower> Doubles = new();
        public List<TowerData> Singles = new();
        

        public void ClearGroups()
        {
            Doubles.Clear();
            Singles.Clear();
        }
    }
    public class LinkGroupMaker
    {
        private LinkGroupData Data = new();
        
        private int[] _towers;
        private uint[] _actors;
        
        public LinkGroupData GetGroups(uint[] actors) //, out HashSet<DoubleTower> turnDoubles, out Dictionary<int, TowerData> singles)
        {
            _actors = actors;
            GroupTowers();
            return Data;
        }
        
        void GroupTowers()
        {
            Data.ClearGroups();
            foreach (var actorID in _actors)
            {
                var actor = ActorHolder.Registry[actorID];
                
                if (actor.Type == ActorType.MultiTower)
                {
                    var doubleTower = AllDoubles.GetDouble(actorID);
                    Data.Doubles.Add(doubleTower);
                    
                }
                else
                {
                    var tower = AllTowers.GetData(actor.TowerIDs[0]);
                    Data.Singles.Add(tower);
                }
            }
        }
    }

}
