using System;
using System.Collections.Generic;
using Towers;
using UnityEngine;


namespace Data
{
    [CreateAssetMenu(fileName = nameof(TowerData))]
    [Serializable]
    public class TowerData : ScriptableObject
    {
        public int UniqID;
        //public string uniqueID = Guid.NewGuid().ToString();
        
        public int SlotId; //sonradan get set eklenebilir
        public float Height;
        public TeamTowerData TeamTowerData;
        public List<int> LinkedTowerIDs;
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
        


    }
}