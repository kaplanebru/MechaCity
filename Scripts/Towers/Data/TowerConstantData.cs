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
        public int DamagePower = 1;
        public int ShieldHeight = 0;
        public int ShotAmount = 1;
        public bool IsDisarmed = false;
        public LockStatus StartLockStatus;
    }

    public class BpStartData //todo: maybe
    {
        public bool HasShield = false;
        public bool CantMove = false;
        public bool CantShoot = false;
        
        public int ShieldHeight = 0;
        public int ShotAmount = 1;
    }
}