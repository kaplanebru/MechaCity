using UnityEngine;

namespace ChainDemo
{
    [CreateAssetMenu(fileName = nameof(PoleData))]
    public class PoleData : ScriptableObject
    {
        public float height = 0.1f;
        public float speed = 0.01f;
        public float limit = 2;
        public float limitDown = 0.5f;
        public float randomAmount = 0.5f;
    }
}
