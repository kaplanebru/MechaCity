using Enums;

namespace Blueprint
{
    public class BpReverse : BaseBlueprint, IBpActionProcessor<ReverseAction>
    {
        public override BpType Type { get; set; } = BpType.Reverse;
        public override int[] SelectedElements { get; set; }
        public ReverseAction BpAction { get; } = new();
        
        public override void TryTakeAction()
        {
            BpAction.Execute(SelectedElements);
        }

        public override void TryRestoreAction()
        {
            throw new System.NotImplementedException();
        }
    }
}

