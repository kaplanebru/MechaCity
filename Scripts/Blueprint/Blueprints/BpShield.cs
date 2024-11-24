
using System.Collections;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Blueprint
{
    public class BpShield: BaseBlueprint, IBpActionProcessor<ShieldAction>
    {
        public ShieldAction BpAction { get; } = new ShieldAction();
        public override BpType Type { get; set; } = BpType.Shield;
        public override SelectionType SelectionType { get; set; } = SelectionType.PlayerOnlyBp;
        public override int Lifespan { get; set; } = 1;//COOLDOWN
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
