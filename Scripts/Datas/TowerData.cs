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
        public int AttackPower = 1;
        public int MaxAttackAmount = 1;
        public TeamData TeamData;
        public List<Tower> LinkedTowers;

        public List<CombatPair> CombatPairs;
        
    }
}
