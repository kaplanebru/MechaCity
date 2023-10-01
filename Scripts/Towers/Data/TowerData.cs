using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class TowerData
    {
        public int UniqID;
        //public string uniqueID = Guid.NewGuid().ToString();
        
        public int SlotId; //sonradan get set eklenebilir
        public float Height;
        public TeamTowerData TeamTowerData;
        public List<Tower> LinkedTowers;
        public bool CanShoot { get; private set; }


        [SerializeField] private int _bulletAmount = 1;
        public int BulletAmount
        {
            get => _bulletAmount;
            set
            {
                _bulletAmount = value;
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
        


    }
}