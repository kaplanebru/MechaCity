using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = nameof(TeamAssetHolder))]
    public class TeamAssetHolder : ScriptableObject
    {
        public TeamType Type;
        public Transform TowersPrefab;
    }
}
