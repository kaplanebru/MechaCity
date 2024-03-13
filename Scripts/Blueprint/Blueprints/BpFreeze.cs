using Enums;

namespace Blueprint
{
    public class BpFreeze : BaseBlueprint, IBpActionProcessor<FreezeAction>
    {
        public override BpType Type { get; set; } = BpType.Freeze;
        public override int Lifespan { get; set; } = 1;
        public FreezeAction BpAction { get; } = new FreezeAction();

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
