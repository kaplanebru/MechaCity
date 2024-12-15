using System.Collections;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Blueprint
{
    public class BpMultiShot : BaseBlueprint, IBpActionProcessor<MultiShotAction>
    {
        public MultiShotAction BpAction { get; } = new MultiShotAction();
        public override BpType Type { get; set; } = BpType.MultiShot;
        public override SelectionType SelectionType { get; set; } = SelectionType.SinglePlayerOnlyBP;
        public override int Lifespan { get; set; } = 1;
        public override bool TryTakeAction(uint[] selectedItems)
        {
            Debug.Log("execute multiShot");
            BpAction.Execute(selectedItems);
            DeselectAfterExecution();
            return true;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
            
        }
    }

}
