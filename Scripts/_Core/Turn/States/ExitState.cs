using System.Collections;
using System.Collections.Generic;
using Enums;
using Testing;
using Turn;
using UnityEngine;

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

    public CombatHelper combatHelper = new();

   

    public override void Subscribe() {}
    
    public override void SubscribeToConstantEvents() {}

    public override void Register()
    {
        combatHelper.Register();
    }

   

    public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
    {
      
        TransferData.Towers = data.Towers;
        combatHelper.Subscribe(TransferData.Towers);
        ExecuteCombat();
    }
    
    void ExecuteCombat()
    {
        // if (MultiplayerSetter.IsTesting)
        // {
        //     Eventbus.CombatEvents.OnCombatTerminated?.Invoke();
        //     combatHelper.Unsubscribe();
        //     return;
        // }
        combatHelper.Fire();
    }
    
    public override void UnsubscribeFromConstantEvents() { }
    public override void Unsubscribe() {}


    
}