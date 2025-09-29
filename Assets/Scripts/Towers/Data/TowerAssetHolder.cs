using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = nameof(TowerAssetHolder))]
    public class TowerAssetHolder : ScriptableObject
    {
        //public Transform Model;
        public Transform HealthIndicator;
    }
}

