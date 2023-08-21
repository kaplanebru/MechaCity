using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
    //[CreateAssetMenu(fileName = nameof(PlayerData))]

    [Serializable]
    public class PlayerData //: ScriptableObject
    {
        public GameGrid Grid;
        public List<Tower> Towers = new();
        public Transform TowersPrefab;
        public TeamData TeamData;

        //public PlayerData RivalData;
    }
}