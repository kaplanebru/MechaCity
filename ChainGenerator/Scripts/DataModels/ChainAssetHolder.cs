using UnityEngine;

namespace Chain
{
    [CreateAssetMenu(fileName = nameof(ChainAssetHolder), menuName = "Chain Generator/" + nameof(ChainAssetHolder))]
    public class ChainAssetHolder : ScriptableObject
    {
        public Cogwheel CogPrefab;
        public LinksPool LinksPoolPrefab;
        public ChainLink lastLinkPrefab;
    }
}

