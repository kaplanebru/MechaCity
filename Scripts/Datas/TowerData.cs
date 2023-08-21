using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public class TowerData
    {
        public int Id { get; set; }
        public int Health;
        public float Height;
        public List<Tower> LinkedTowers;
        public bool CanShoot { get; private set; }

        private int bulletAmount;
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