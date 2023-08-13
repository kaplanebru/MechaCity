using System;
using System.Collections.Generic;
using UnityEngine;


namespace Datas
{
    [Serializable]
    public class TowerData
    {
        public int Id;
        public float Height;
        public TeamData TeamData;
        public List<Tower> TargetTowers;
    }
}
