using System;
using System.Collections.Generic;
using Blueprint;
using Enums;
using UnityEngine;


namespace Towers
{
    // [CreateAssetMenu(fileName = nameof(TowerData))]
    [Serializable]
    public class TowerData
    {
        public int UniqID;
        
        public float Height;

        public int SlotId;
        public TeamType TeamType;
        public List<int> LinkedTowerIDs = new();
        public bool CanShoot { get; private set; }
        
        [SerializeField] private int _bulletAmountt = 1;
        public int BulletAmount
        {
            get => _bulletAmountt;
            set
            {
                _bulletAmountt = value;
                CanShoot = value > 0;
            }
        }

        [SerializeField] int _health = 1;

        public int Health
        {
            get => _health;
            set => _health = value;
            //CanShoot = value > 0;
        }

        public int DamagePower;
        public bool IsClickable = true;
        public BpTowerData BpTowerData;
        
    }

    
}


