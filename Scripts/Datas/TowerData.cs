using System;
using System.Collections.Generic;
using Models;
using UnityEngine;


namespace Datas
{
    [Serializable]
    public class TowerData
    {
        public int Id;
        public float Height;
        public int DamagePower = 1;
        public int MaxBullet = 1;
        public TeamData TeamData;
        public List<Tower> LinkedTowers;
    }
}
