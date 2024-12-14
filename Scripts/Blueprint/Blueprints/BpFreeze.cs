using Enums;
using Enums.Selections;
using UnityEngine;

namespace Blueprint
{
    public class BpFreeze : BaseBlueprint, IBpActionProcessor<FreezeAction> //bunlar scriptable olabilird, bpAction native olurdu? / bpnin monobehaviour olması gerekirdi
    {
        public override BpType Type { get; set; } = BpType.Freeze;
        public override SelectionType SelectionType { get; set; } = SelectionType.SingleRivalOnlyBP;
        public override int Lifespan { get; set; } = 1;  //dışardan belirlenmeli - değişken
        public override int MaxSelectionAmount { get; set; } = 1; //dışardan belirlenmeli - değişken
        public FreezeAction BpAction { get; } = new FreezeAction();

        public override bool TryTakeAction(uint[] selectedItems)
        {
            Debug.Log("EXECUTE freeze");
            BpAction.Execute(selectedItems);
            DeselectAfterExecution();
            return true;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
            BpAction.Restore(selectedItem);
        }
        
    }
}
