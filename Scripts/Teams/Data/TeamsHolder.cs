using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teams;

namespace Data
{
    [CreateAssetMenu(fileName = nameof(TeamsHolder))]
    public class TeamsHolder : ScriptableObject
    {
        public Team[] Teams;
    }
}
