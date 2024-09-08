using System.Collections;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Blueprint
{
    public class BpDouble : BaseBlueprint, IBpActionProcessor<DoubleAction>
    {
        public override BpType Type { get; set; } = BpType.Double;
        public override SelectionType SelectionType { get; set; } = SelectionType.All;
        public override int Lifespan { get; set; } = 1;
        public override int MaxSelectionAmount { get; set; } = 1;
        public DoubleAction BpAction { get; } = new DoubleAction();
        
        
        public override bool TryTakeAction(int[] selectedItems)
        {
            BpAction.Execute(selectedItems);
            return true;
        }

        public override void TryRestoreAction(int selectedItem)
        {
            BpAction.Restore(selectedItem);
        }
    }

}

