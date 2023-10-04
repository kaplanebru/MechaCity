using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using Grid;
using PlayerNetwork;
using Towers;

namespace Data
{
    [CreateAssetMenu(fileName = nameof(TeamData))]

    [Serializable]
    public class TeamData: ScriptableObject
    {
        public string Name;
        public TeamType TeamType;
        public Player Player;
        public List<int> TowerIds = new();
        public GameGrid Grid;
        public TowersDataHolder towerDatas;


        public TeamTowerData TeamTowerData;
        //public Towers<TowerData> TowerDatas;
    }
    
}