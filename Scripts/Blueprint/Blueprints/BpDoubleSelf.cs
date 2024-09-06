using System.Collections;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Blueprint
{
    public class BpDoubleSelf : BaseBlueprint, IBpActionProcessor<DoubleSelfAction>
    {
        public override BpType Type { get; set; } = BpType.DoubleSelf;
        public override SelectionType SelectionType { get; set; } = SelectionType.PlayerOnlyBp;
        public override int Lifespan { get; set; } = 1;
        public override int MaxSelectionAmount { get; set; } = 1;
        public DoubleSelfAction BpAction { get; } = new DoubleSelfAction();
        
        
        public override void TryTakeAction(int[] selectedItems)
        {
            BpAction.Execute(selectedItems);
        }

        public override void TryRestoreAction(int selectedItem)
        {
            BpAction.Restore(selectedItem);
        }
    }
}