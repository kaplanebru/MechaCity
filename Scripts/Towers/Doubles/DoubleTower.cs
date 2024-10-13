using System;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using UnityEngine;

namespace Towers
{
    public class DoubleTower: ILinkable
    {
        [NonSerialized]
        public Dictionary<int, TowerData> towers = new();
       
        public int Amount { get; set; } //private set?
        public int ID { get; }

        public DTPhysical Physical;
      
        
        public int GetFreeResource(int step) =>  Amount * step;
        public int AvailableHeight //1-3'se mesela inemesin
        {
            get
            {
                return towers.Sum(tower => tower.Value.AvailableHeight);
            }
        }

        public DoubleTower(params int[] ids)
        {
            foreach (var id in ids)
            {
                towers.Add(id, AllTowers.GetData(id));
            }
            
            RegisterActor();
            
            towers = towers.OrderBy(t => t.Value.AvailableHeight).ToDictionary(t => t.Key, t => t.Value);
            Amount = towers.Count;

            ID = UniqueIdGenerator.IntId();
            Physical = new DTPhysical(towers);
        }
        
        private void RegisterActor()
        {
            Eventbus.ActorEvents.OnNewDoubleActor.Invoke(towers.Keys.ToArray());
        }
        public bool InspectByTowerData(TowerData tower) => towers.ContainsValue(tower);
        public bool InspectByTowerID(int id) => towers.ContainsKey(id);
        
        public bool NoDoubleFallCapacity(int step)
        {
            return towers.ElementAt(0).Value.AvailableHeight < step;
        }
        public void DoubleFallOperation(int step)
        {
            foreach (var tower in towers.Values)
            {
                tower.UpdateHeight(-step);
            }

            //MediatorEventbus.ChainMotionEvents.OnRising?.Invoke(); //TODO: 2 kez çağrılıyor olabilir
        }
        
        public bool Same(ILinkable other)
        {
            return other == this;
        }
    }
}