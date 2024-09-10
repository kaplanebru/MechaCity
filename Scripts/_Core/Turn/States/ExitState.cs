using System.Collections.Generic;
using Enums;
using Enums.Selections;
using Turn;

public class ExitTransferData : BaseTurnTransferData
{
    public override TurnStateType StateType { get; set; } = TurnStateType.Exit;
    public override List<int> Towers { get; set; } = new();
}

public class ExitState : BaseTurnState, ITransferDataHolder<ExitTransferData>
{
    public ExitTransferData TransferData { get; } = new();
    public override int StateId { get; set; }
    public override TurnStateType StateType { get; } = TurnStateType.Exit;

    public CombatOperator CombatOperator;

    public override void Subscribe()
    {
        SelectionEvents.OnSelectionTerminated?.Invoke();
    }
    public override void SubscribeToConstantEvents() {}
    public override void Register() {}

    public void GetCombatHelper(CombatOperator combatOperator)
    {
        CombatOperator = combatOperator;
    }
    public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
    {
        TransferData.Towers = data.Towers;
        CombatOperator.Subscribe(TransferData.Towers);
        ExecuteCombat();
    }
    
    void ExecuteCombat()
    {
        CombatOperator.Fire();
    }
    
    public override void UnsubscribeFromConstantEvents() { }
    public override void Unsubscribe() {}


    
}