using Data;
using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = nameof(TowersDataHolder))]
    public class TowersDataHolder : ScriptableObject
    {
        public TowerConstantData[] Datas;
        
    }
}

