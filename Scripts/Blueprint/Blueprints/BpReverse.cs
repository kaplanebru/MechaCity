using Enums;

namespace Blueprint
{
    public class BpReverse : BaseBlueprint, IBpActionProcessor<ReverseAction>
    {
        public override BpType Type { get; set; } = BpType.Reverse;
        public ReverseAction BpAction { get; } = new();
        
        public override void TryTakeAction()
        {
            BpAction.Execute();
        }
    }
}

