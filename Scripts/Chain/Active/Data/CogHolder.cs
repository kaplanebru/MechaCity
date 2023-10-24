using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [CreateAssetMenu(fileName = nameof(CogHolder))]
    public class CogHolder : ScriptableObject
    {
        public Cogwheel[] Cogs;
    }
}

