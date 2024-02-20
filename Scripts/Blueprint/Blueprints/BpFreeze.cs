using Enums;

namespace Blueprint
{
    public class BpFreeze : BaseBlueprint, IBpActionProcessor<FreezeAction>
    {
        public FreezeAction BpAction { get; }
        public override BpType Type { get; set; }
       
        public override void TryTakeAction()
        {
            BpAction.Execute();
        }
    }
}
