using UnityEngine;
using ProjectileHandler;

namespace Data
{
    [CreateAssetMenu(fileName = nameof(TowerAssetHolder))]
    public class TowerAssetHolder : ScriptableObject
    {
        //public Transform Model;
        public Transform HealthIndicator;
        public Projectile ProjectileObject; //temp
    }
}

