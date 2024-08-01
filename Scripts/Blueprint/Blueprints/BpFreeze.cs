using Enums;
using UnityEngine;

namespace Blueprint
{
    public class BpFreeze : BaseBlueprint, IBpActionProcessor<FreezeAction> //bunlar scriptable olabilird, bpAction native olurdu? / bpnin monobehaviour olması gerekirdi
    {
        public override BpType Type { get; set; } = BpType.Freeze;
        public override SelectionType SelectionType { get; set; } = SelectionType.RivalOnly;
        public override int Lifespan { get; set; } = 1;  //dışardan belirlenmeli - değişken
        public override int MaxSelectionAmount { get; set; } = 1; //dışardan belirlenmeli - değişken
        public FreezeAction BpAction { get; } = new FreezeAction();

        public override void TryTakeAction(int[] selectedItems)
        {
            Debug.Log("EXECUTE");
            BpAction.Execute(selectedItems);
            
        }

        public override void TryRestoreAction(int selectedItem)
        {
            BpAction.Restore(selectedItem);
        }
        
    }
}
