using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    public class ChainGeneratorData
    {
        public ChainData ChainData;
        public LinksPool Pool;
        public List<Vector3> ChainPoints;

        public ChainGeneratorData(ChainData chainData, LinksPool pool, List<Vector3> chainPoints = null)
        {
            ChainData = chainData;
            Pool = pool;
            if(chainPoints == null)
                ChainPoints = new List<Vector3>();
        }
    }
}

