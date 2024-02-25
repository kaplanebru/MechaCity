using System;
using System.Collections.Generic;


namespace Turn
{
    [Serializable]
    public abstract class BaseTurnTransferData
    {
        public abstract List<int> Towers { get; set; }

    }
}

