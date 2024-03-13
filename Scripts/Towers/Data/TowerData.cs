using System;
using System.Collections.Generic;
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
        public TeamTowerData TeamTowerData;
        public List<int> LinkedTowerIDs = new();
        public bool CanShoot { get; private set; }
     

        // private bool isClickable = true;
        // public bool IsClickable
        // {
        //     get => isClickable;
        //     set
        //     {
        //         isClickable = value;
        //         if (!isClickable)
        //         {
        //             Eventbus.
        //         }
        //     }
        // }


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

        public TowerData(int uniqID)// float height, int health, int bulletAmount, int damagePower) //damage amount later
        {
            UniqID = uniqID;
            // Height = fightData.height;
            // Health = fightData.health;
            // BulletAmount = fightData.bulletAmount;
            // DamagePower = fightData.damagePower;
        }
    }
    
    // [Serializable]
    // public class TowerFightData
    // {
    //     public float height;
    //     public int health;
    //     public int bulletAmount;
    //     public int damagePower;
    // }

    
}


