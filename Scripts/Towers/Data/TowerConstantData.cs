using System;
using UnityEngine;


namespace Data
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(TowerConstantData))]
    public class TowerConstantData : ScriptableObject
    {
        public TowerAssetHolder TowerAssetHolder; //TODO: fix later
        //public TowerFightData FightData;
        public float StartHeight = 2;
        public int StartHealth = 1;
        public int DamagePower = 1;
        public int MaxBullet = 1;
    }
}
