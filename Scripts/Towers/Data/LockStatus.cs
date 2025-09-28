using System;

namespace Towers
{
    [Serializable]
    public class LockStatus
    {
        public bool Locked = false;
        public int Limit = 1;
    }
}
