using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public class TowerData
    {
        public int Id; //sonradan get set eklenebilir
        public int Health;
        public float Height;
        public TeamTowerData TeamTowerData;
        public List<Tower> LinkedTowers;
        public bool CanShoot { get; private set; }

        private int bulletAmount;
        public bool Clickable = true;
        public int BulletAmount
        {
            get => bulletAmount;
            set
            {
                bulletAmount = value;
                CanShoot = value > 0;
            }
        }

        
    }
}