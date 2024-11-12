using System;
using Actor;
using Towers;


namespace Grid
{
    [Serializable]
    public class Slot
    {
        public int Id;
        public int[] RelatedSlots;
        
        public ActorData Actor { get; set; }
    }

}
