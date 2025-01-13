using Enums;
using Enums.Selections;
using UnityEngine;

namespace Blueprint
{
    public class BpReverse : BaseBlueprint, IBpActionProcessor<ReverseAction>
    {
        public override BpType Type { get; set; } = BpType.Reverse;
        public override SelectionType SelectionType { get; set; } = SelectionType.None;
        public override int Lifespan { get; set; } = 1;
        public ReverseAction BpAction { get; } = new();
        
        public override bool TryTakeAction(uint[] selectedItems)
        {
            if (IsPlaying) return false;
            IsPlaying = true;
            
            BpAction.Execute();
            return true;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
            BpAction.Restore(selectedItem);
        }
    }
}

