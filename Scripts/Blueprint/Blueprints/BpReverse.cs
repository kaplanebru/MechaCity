using Enums;

public class BpReverse : BaseBlueprint, IBpActionProcessor<ReverseAction>
{
    public ReverseAction BpAction { get; } = new();

    public override BpType Type { get; set; } = BpType.Reverse;
   
}