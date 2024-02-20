using Enums;

public class BpFreeze : BaseBlueprint, IBpActionProcessor<FreezeAction>
{
    public FreezeAction BpAction { get; }
    public override BpType Type { get; set; }
}