using System;
using System.Collections.Generic;
using DataModels;
using UnityEngine;


namespace Data
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(TowerConstantData))]
    public class TowerConstantData : ScriptableObject
    {
        public TowerAssetHolder TowerAssetHolder;
        public float StartHeight = 2;
        public int StartHealth = 5;
        public int DamagePower = 1;
        public int MaxBullet = 1;
    }
}
