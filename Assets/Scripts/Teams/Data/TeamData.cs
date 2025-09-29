using System;
using System.Collections.Generic;
using Actor;
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
        public List<ActorData> Actors = new();
        
        public TeamColorData teamColorData;
       
    }
    
}