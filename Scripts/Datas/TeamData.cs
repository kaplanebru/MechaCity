using System;
using System.Collections;
using System.Collections.Generic;
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
        public List<Tower> Towers = new();
        public GameGrid Grid;


        public TeamTowerData TeamTowerData;
    }
    
}