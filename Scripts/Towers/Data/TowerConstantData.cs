using System;
using Enums;
using UnityEngine;


namespace Towers
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(TowerConstantData))]
    public class TowerConstantData : ScriptableObject
    {
        public TowerAssetHolder TowerAssetHolder; //TODO: fix later
        public int StartHeight = 2;
        public int StartHealth = 1;
        public int DamagePower = 1;
        public int MaxBullet = 1;
        public TeamType StartTeam;
        public LockStatus StartLockStatus;
    }
}