using System;
using Towers;


namespace Grid
{
    [Serializable]
    public class Slot
    {
        public bool HasTower = true;
        public int Id;
        public Tower Tower;
    }

}
