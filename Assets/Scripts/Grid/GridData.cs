using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(menuName = "Grid/" + nameof(GridData), fileName = nameof(GridData))]
    public class GridData : ScriptableObject
    {
        public Slot[] slots;
        public InterruptionCouple[] interruptions;
    }
}
