using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Towers;
using UnityEngine;

namespace Actor
{
    public class ActivityStatus
    {
        public bool CanMove = true;
        public bool CanShoot = true;
    }
    
    [Serializable]
    public class ActorData
    {
        public uint ID { get; set; }
       
        public ActorType Type;
        public TeamType TeamType;
        public TeamColorData TeamVisualData;
        public int Row;
        
       
        public int InitialHealth;
        public int Health { get; set; }
        public TowerData[] Towers { get; set; }
        public int[] TowerIDs { get; set; }
        public int TowerAmount { get; set; }
        public Vector3 Center { get; set; }
        
        public HashSet<uint> TargetActors = new();
        public HashSet<uint> Neighbours = new();
        public ActivityStatus ActivityStatus = new();
        public ActorData(uint id, ActorType type, params int[] towerIDs)
        {
            ID = id;
            Type = type;

            RegisterTowersByIDs(towerIDs);
            SetCenter();
        }
        
        void RegisterTowersByIDs(params int[] towerIDs)
        {
            TowerIDs = towerIDs;
            Towers = new TowerData[TowerIDs.Length];
            TowerAmount = Towers.Length;
            
            for (var i = 0; i < TowerIDs.Length; i++)
            {
                TowerData tower = AllTowers.GetData(TowerIDs[i]);
                Towers[i] = tower;
            }
            
            OrderTowerDataByHeight();
        }

        internal void OrderTowerDataByHeight()
        {
            Towers = Towers.OrderBy(t => t.AvailableHeight).ToArray(); //İD'NİN LİNKAGE İÇİN YER DEĞİŞTİRMEMESİ Gerekebilir

        }
        
        void SetCenter()
        {
            Center = Vector3.zero;
            foreach (var tower in TowerIDs)
            {
                Center += AllTowers.GetTower(tower).transform.position;
            }

            Center /= TowerAmount;
        }
        public int GetFreeResource(int step) =>  TowerAmount * step;
        public int TryGetAvailableHeight(int step)
        {
            int availableHeight = Towers.Sum(tower => tower.AvailableHeight);
            return Towers[0].AvailableHeight < step ? 0 : availableHeight;
        }
    }
}