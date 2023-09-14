using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = nameof(TeamsHolder))]
    public class TeamsHolder : ScriptableObject
    {
        public Team[] Teams;
    }
}
