using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = nameof(TowerAssetHolder))]
    public class TowerAssetHolder : ScriptableObject
    {
        public Transform Model;
        public Transform HealthIndicator;
    }
}

