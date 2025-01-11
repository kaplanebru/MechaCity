using System.Collections;
using System.Collections.Generic;
using Blueprint;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Blueprint
{
    public class BpDisarmament :BaseBlueprint, IBpActionProcessor<DisarmamentAction>
    {
        public DisarmamentAction BpAction { get; } = new DisarmamentAction();
        public override BpType Type { get; set; } = BpType.Disarmament;
        public override SelectionType SelectionType { get; set; } = SelectionType.SingleRivalOnlyBP;
        public override int Lifespan { get; set; } = 1;
        public override bool TryTakeAction(uint[] selectedItems)
        {
            if (IsActive) return false;
            IsActive = true;
            
            BpAction.Execute(selectedItems);
            DeselectItems();
            return true;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
            
        }
    }

}

