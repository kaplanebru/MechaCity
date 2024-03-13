using Enums;

namespace Blueprint
{
    public class BpReverse : BaseBlueprint, IBpActionProcessor<ReverseAction>
    {
        public override BpType Type { get; set; } = BpType.Reverse;
        public override int Lifespan { get; set; } = 1;
        public ReverseAction BpAction { get; } = new();
        
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

