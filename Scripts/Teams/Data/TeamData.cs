using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using Grid;
using PlayerNetwork;
using Towers;

namespace Teams
{
    [CreateAssetMenu(fileName = nameof(TeamData))]

    [Serializable]
    public class TeamData: ScriptableObject
    {
        public string Name;
        public TeamType TeamType;
        public Player Player;
        public List<TowerData> Towers = new();
        
        public TeamColorData teamColorData;
       
    }
    
}