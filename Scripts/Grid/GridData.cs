using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(menuName = "Grid/" + nameof(GridData), fileName = nameof(GridData))]
    public class GridData : ScriptableObject
    {
        public Slot[] slots;
        public InterruptionCouple[] interruptions;
        public Dictionary<int, InterruptionCouple> InterruptionCouplesByID = new();

        public void Setup()
        {
            foreach (var interruptionCouple in interruptions)
            {
                InterruptionCouplesByID.Add(interruptionCouple.id, interruptionCouple);
            }
        }
    }
}
