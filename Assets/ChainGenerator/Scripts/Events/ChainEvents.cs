using System;
using ChainInGame;

namespace Chain
{
    public class ChainEvents
    {
        public static Action<float, int> OnCogSpeedSet;
        public static Action OnMovieClipBegin;

        public static Action<int> OnCreateTeethPool;

        public class InGameEvents
        {
            public static Action<Machinery[]> OnMachineriesSet;
            public static Action<int> OnOptionSet;
        }
    }
}
