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
    public class ActorData
    {
        public uint ID;
        public int Row;
        public ActorType Type;
        
        public int[] TowerIDs;
        public TowerData[] Towers;
        public int TowerAmount { get; set; }
        public Vector3 Center;
        
        public int Health;
        public int InitialHealth;
        
        public HashSet<uint> TargetActors = new();
        public HashSet<uint> Neighbours = new();
        public ActivityStatus ActivityStatus = new();
        public ActorData(uint id, ActorType type, params int[] towerIDs)
        {
            ID = id;
            Type = type;

            SetTowers(towerIDs);
            SetCenter();
        }

        void SetTowers(params int[] towerIDs)
        {
            TowerIDs = towerIDs;//towerIDs.OrderBy(i=>i).ToArray();
            Towers = new TowerData[TowerIDs.Length]; //TODO: make dict int,Data
            TowerAmount = Towers.Length;
          

            for (var i = 0; i < TowerIDs.Length; i++)
            {
                TowerData tower = AllTowers.GetData(TowerIDs[i]);
                Towers[i] = tower;
            }
            Towers = Towers.OrderBy(t => t.AvailableHeight).ToArray(); //İD'NİN LİNKAGE İÇİN YER DEĞİŞTİRMEMESİ Gerekebilir
        }
        
        

        void SetCenter()
        {
            Center = Vector3.zero;
            foreach (var tower in TowerIDs)
            {
                Center += AllTowers.GetTower(tower).transform.position;
            }

            //HealthParent = AllTowers.GetTower(Towers.Last().UniqID).transform; //todo: health holderı almalı aslında, towerı değil
            Center /= TowerAmount;
           // Center.y = HealthParent.position.y;
        }
        public int GetFreeResource(int step) =>  TowerAmount * step;
        public int TryGetAvailableHeight(int step)
        {
            int availableHeight = Towers.Sum(tower => tower.AvailableHeight);
            return Towers[0].AvailableHeight < step ? 0 : availableHeight;
        }
    }
}