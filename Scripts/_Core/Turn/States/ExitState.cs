using System.Collections;
using System.Collections.Generic;
using Enums;
using Turn;
using UnityEngine;

public class ExitTransferData : BaseTurnTransferData
{
    public override TurnStateType StateType { get; set; } = TurnStateType.Exit;
    public override List<int> Towers { get; set; } = new();
}

public class ExitState : BaseTurnState, ITurnTransferHandler<ExitTransferData>
{
    public ExitTransferData TransferData { get; } = new();
    public override int StateId { get; set; }
    public override TurnStateType StateType { get; } = TurnStateType.Exit;

    public CombatHelper combatHelper = new();
    
    public override void Subscribe() {}
    public override void Unsubscribe() {}

    public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
    {
        TransferData.Towers = data.Towers;
        combatHelper.Subscribe(TransferData.Towers, turnManager);
        ExecuteCombat();
    }
    
    void ExecuteCombat()
    {
        combatHelper.Fire();
    }
    
    private void EndTurn()
    {
        combatHelper.Unsubscribe();
    }
    
   



}