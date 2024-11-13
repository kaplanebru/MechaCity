using System.Collections.Generic;
using System.Linq;
using Enums;
using Towers;
using UnityEngine;

namespace Actor
{
    public class ActorData
    {
        public uint ID;
        public int Row;
        public ActorType Type;
        public int[] TowerIDs;
        public TowerData[] Towers;
        //public Transform HealthParent;
        public int TowerAmount { get; set; }
        public Vector3 Center;
        
        public int Health;
        public int InitialHealth;
        
        public List<uint> TargetActors = new();
        public List<uint> Neighbours = new();

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
        

       

        public void SetNeighbours(params uint[] neighbours)
        {
            Neighbours = neighbours.ToList();
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