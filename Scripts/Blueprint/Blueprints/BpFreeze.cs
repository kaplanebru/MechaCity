using Enums;

namespace Blueprint
{
    public class BpFreeze : BaseBlueprint, IBpActionProcessor<FreezeAction>
    {
        public override BpType Type { get; set; }
        public FreezeAction BpAction { get; }
    }
}
