using System;
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
        public int[] Interrupter;
        public int Interrupted;
    }

}
