using System;
using System.Collections.Generic;
using Actor;
using Towers;
using UnityEngine;


namespace Grid
{
    [Serializable]
    public class Slot
    {
        public int Id;
        public int[] TargetSlots;
        public int[] ReversedTargetSlots;
        public int[] Neighbours;
    }

    [Serializable]
    public class InterruptionCouple
    {
        public int id;
        public int[] Interrupters;
        public int Interrupted;
        
        public Vector3 Offset;
    }

    public class InterruptionByActor
    {
        public int id;
        public List<uint> Interrupters = new();
        public uint Interrupted;
        public Vector3 Offset;
    }

}
