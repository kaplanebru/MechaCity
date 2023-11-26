using UnityEngine;

namespace Chain
{
    [CreateAssetMenu(fileName = nameof(ChainAssetHolder))]
    public class ChainAssetHolder : ScriptableObject
    {
        public Cogwheel CogPrefab;
        public LinksPool LinksPoolPrefab;
        public ChainLink lastLinkPrefab;
    }
}

