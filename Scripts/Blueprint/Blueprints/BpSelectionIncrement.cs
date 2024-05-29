using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class BpSelectionIncrement: BaseBlueprint, IBpActionProcessor<SelectionIncrementAction>
    {
        public override BpType Type { get; set; } = BpType.SelectionIncrement;
        public override SelectionType SelectionType { get; set; } = SelectionType.None;
        public override int Lifespan { get; set; } = 1;
        public override int MaxSelectionAmount { get; set; } = 0;
    
        public SelectionIncrementAction BpAction { get; } = new();
        public override void TryTakeAction(int[] selectedItems)
        {
            BpAction.Execute();
        }

        public override void TryRestoreAction(int selectedItem)
        {
            BpAction.Restore(selectedItem);
        }
    }
}
