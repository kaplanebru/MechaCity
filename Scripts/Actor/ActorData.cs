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


        public int InitialHealth = 1;
        public int Health { get; set; }

        public TowerNumericData[] TowerNumericDatas { get; set; }
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

            RegisterTowersDependently(towerIDs);
            SetCenterDependently();
        }

        void RegisterTowersDependently(params int[] towerIDs)
        {
            TowerIDs = towerIDs;
            Towers = AllTowers.GetTowerDatasByIDs(towerIDs).ToArray();
            TowerAmount = Towers.Length;
            OrderTowerDataByHeight();
        }

        
        public void OrderTowerDataByHeight()
        {
            TowerNumericDatas = TowerNumericDatas.OrderBy(t => t.AvailableHeight).ToArray();
            Towers = Towers.OrderBy(t => t.NumericData.AvailableHeight).ToArray();
        }
        
        internal void SetCenterDependently()
        {
            Center = Vector3.zero;
            foreach (var tower in TowerIDs)
            {
                Center += AllTowers.GetTowerPos(tower);
            }

            Center /= TowerAmount;
        }
        
        internal void SetCenterAutonomously(TowerObject[] towerObjects)
        {
            Center = Vector3.zero;
            foreach (var towerObject in  towerObjects)
            {
                Center += towerObject.transform.position;
            }
            
            Center /= TowerAmount;
        }

        public int GetTowerAmountsPlusStep(int step) => TowerAmount * step;

        public int TryGetAvailableHeightByStep(int step)
        {
            int availableHeight = TowerNumericDatas.Sum(tower => tower.AvailableHeight);
            return TowerNumericDatas[0].AvailableHeight < step ? 0 : availableHeight;
        }

        public int GetTotalHeight()
        {
            return TowerNumericDatas.Sum(tower => tower.Height);
        }
    }
}