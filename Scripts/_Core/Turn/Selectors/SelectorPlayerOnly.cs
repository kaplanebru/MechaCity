using Enums;
using Towers;
using UnityEngine;

namespace _Core.Turn.Selectors
{
    public class SelectorWithBlocker<TBlocker> : Selector<StandardSelectionColor> where TBlocker : ITeamBlocker, new()
    {
        public TBlocker Blocker = new TBlocker();
    }
    
    public class BpSelectorWithBlocker<TBlocker>:  Selector<BpSelectionColor> where TBlocker : ITeamBlocker, new()
    {
        public TBlocker Blocker = new TBlocker();
    }
    
    public class SelectorBoth : Selector<StandardSelectionColor>
    {
        //bunun kuralları daha farklı olacak zaten
    }
}