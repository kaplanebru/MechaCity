using Enums;
using Towers;
using UnityEngine;

namespace _Core.Turn.Selectors
{
    public class SelectorPlayerOnly : BaseSelector<StandardSelectionColor>
    {
        public SelectionBlocker<RivalBlocker> SelectionBlocker = new ();
    }
    
    public class SelectorRivalOnly : BaseSelector<StandardSelectionColor>
    {
        public SelectionBlocker<PlayerBlocker> SelectionBlocker = new();
    }

    public class BpSelectorPlayerOnly: BaseSelector<BpSelectionColor>
    {
        public SelectionBlocker<RivalBlocker> SelectionBlocker = new();
    }

    public class BpSelectorRivalOnly : BaseSelector<BpSelectionColor>
    {
        public SelectionBlocker<PlayerBlocker> SelectionBlocker = new();
    }
    
    
    
    public class SelectorBoth : BaseSelector<StandardSelectionColor>
    {
        //bunun kuralları daha farklı olacak zaten
    }
}