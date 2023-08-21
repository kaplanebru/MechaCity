using System;
using System.Collections.Generic;
using Models;
using UnityEngine;


namespace Datas
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(TowerConstantData))]
    public class TowerConstantData : ScriptableObject
    {
        public float StartHeight = 2;
        public Transform Model;
        public int StartHealth = 5;
        public int DamagePower = 1;
        public int MaxBullet = 1;
    }
}
