using System.Collections;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Blueprint
{
    public class BpBreakShield :BaseBlueprint, IBpActionProcessor<BreakShieldAction>
    {
        public BreakShieldAction BpAction { get; } = new BreakShieldAction();
        public override BpType Type { get; set; } = BpType.BreakShield;
        public override SelectionType SelectionType { get; set; } = SelectionType.SingleRivalOnlyBP;
        public override int Lifespan { get; set; } = 1;
        public override int MaxSelectionAmount { get; set; } = 1;
        public override bool TryTakeAction(uint[] selectedItems)
        {
            BpAction.Execute(selectedItems);
            return true;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
            
        }
    }
}

