using System.Collections.Generic;
using Enums;
using Enums.Selections;
using Turn;
using UnityEngine;

public class ExitTransferData : BaseTurnTransferData
{
    public override TurnStateType StateType { get; set; } = TurnStateType.Exit;
    public override List<uint> Actors { get; set; } = new();
}

public class ExitState : BaseTurnState, ITransferDataHolder<ExitTransferData>
{
    public ExitTransferData TransferData { get; } = new();
    public override int StateId { get; set; }
    public override TurnStateType StateType { get; } = TurnStateType.Exit;

    public CombatOperator CombatOperator;

    public override void Subscribe()
    {
        //SelectionEvents.OnSelectionTerminated?.Invoke();
        SelectionEvents.OnSelectionTerminated?.Invoke();
    }

    public override void SubscribeToConstantEvents()
    {
        Eventbus.ActorEvents.OnReverseGrid += ReverseCombatDirection;

    }

    private void ReverseCombatDirection()
    {
        CombatOperator.ReverseCombatDirection();
    }

    public override void Register() {}

    public void SetCombatOperator(CombatOperator combatOperator)
    {
        CombatOperator = combatOperator;
        

    }
    public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
    {
        TransferData.Actors = data.Actors;
        CombatOperator.Setup(TransferData.Actors);
        ExecuteCombat();
    }
    
    void ExecuteCombat()
    {
        CombatOperator.Fire();
    }

    public override void UnsubscribeFromConstantEvents()
    {
        Eventbus.ActorEvents.OnReverseGrid -= ReverseCombatDirection;
    }
    public override void Unsubscribe() {}


    
}