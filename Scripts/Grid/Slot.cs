using System;
using System.Collections.Generic;
using Actor;
using Towers;


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
    }

    public class InterruptionByActor
    {
        public int id;
        public List<uint> Interrupters = new();
        public uint Interrupted;
    }

}
