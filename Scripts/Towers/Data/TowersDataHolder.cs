using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = nameof(TowersDataHolder))]
    public class TowersDataHolder : ScriptableObject
    {
        [SerializeField]TowerData[] Datas;

        public TowerData GetTowerData(int id) => Datas[id];
    }
}

