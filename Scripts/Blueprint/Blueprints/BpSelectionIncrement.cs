using System.Collections;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Blueprint
{
    public class BpSelectionIncrement: BaseBlueprint, IBpActionProcessor<SelectionIncrementAction>
    {
        public override BpType Type { get; set; } = BpType.SelectionIncrement;
        public override SelectionType SelectionType { get; set; } = SelectionType.None;
        public override int Lifespan { get; set; } = 1;
    
        public SelectionIncrementAction BpAction { get; } = new();
        public override bool TryTakeAction(uint[] selectedItems)
        {
            if (IsPlaying) return false;
            IsPlaying = true;
            
            Debug.Log("EXECUTE");
            BpAction.Execute();
            DeselectItems();
            return true;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
            BpAction.Restore(selectedItem);
        }
    }
}
