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
        public HashSet<DoubleTower> TurnDoubles = new();
        //public Dictionary<int, TowerData> Singles = new();
        public List<TowerData> Singles = new();

        public void ClearGroups()
        {
            TurnDoubles.Clear();
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
                    Data.TurnDoubles.Add(AllDoubles.GetDouble(actorID));
                }
                else
                {
                    Data.Singles.Add(AllTowers.GetData(actor.Towers[0]));
                }
            }
        }
    }

}
